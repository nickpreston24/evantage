using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMechanic.Airtable;
using CodeMechanic.Async;
using CodeMechanic.Diagnostics;
using CodeMechanic.FileSystem;
using CodeMechanic.Types;
using Dapper;
using evantage.Models;
using MySqlConnector;

namespace evantage.Pages.Logs;

public class GlobalLoggingService : IGlobalLoggingService
{
    private async Task<List<CreateTableInfo>> GetTableSchema(string table_name)
    {
        // string query = $"EXPLAIN {table_name}";
        string query = $"show create table {table_name}";

        string connectionstring = await GetConnectionString();
        using var connection = new MySqlConnection(connectionstring);

        var results = connection
            .Query<CreateTableInfo>(query
                , commandType: CommandType.Text
            )
            .ToList();

        results.Dump("table schema");
        return results;
    }
    


    public async Task<List<LogRecord>> GetAllLogs()
    {
        string query =
                """
                    select operation_name,
                           exception_text,
                           payload,
                           diff,
                           sql_parameters,
                           table_name,
                           breadcrumb
                    from logs
                    order by created_at asc, modified_at asc;
                """
            ;
        string connectionstring = await GetConnectionString();
        using var connection = new MySqlConnection(connectionstring);

        var results = connection
            .Query<LogRecord>(query
                , commandType: CommandType.Text
            )
            .ToList();

        int count = results.Count;
        return results;
    }

    public async Task<string> GetConnectionString()
    {
        var connectionString = new MySqlConnectionStringBuilder()
        {
            Database = Environment.GetEnvironmentVariable("MYSQLDATABASE"),
            Server = Environment.GetEnvironmentVariable("MYSQLHOST"),
            Password = Environment.GetEnvironmentVariable("MYSQLPASSWORD"),
            UserID = Environment.GetEnvironmentVariable("MYSQLUSER"),
            Port = (uint)Environment.GetEnvironmentVariable("MYSQLPORT").ToInt()
        }.ToString();

        if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
        return connectionString;
    }

    public async Task<List<LogRecord>> BulkUpsertLogs(List<LogRecord> logRecords)
    {
        var batch_size =
            1000; //(int)Math.Round(Math.Log2(logRecords.Count * 1.0) * Math.Log10(logRecords.Count) * 100, 1);
        Console.WriteLine("batch size :>> " + batch_size);
        var Q = new SerialQueue();
        Console.WriteLine("Running Q of bulk upserts ... ");
        Stopwatch watch = new Stopwatch();
        watch.Start();
        var tasks = logRecords
            .Batch(batch_size)
            .Select(log_batch => Q
                .Enqueue(async () => await UpsertLogs(log_batch, debug_mode: false)));

        await Task.WhenAll(tasks);
        watch.Stop();
        // watch.PrintRuntime($"Done upserting {logRecords.Count} logs! ");
        return logRecords;
    }

    public async Task<List<LogRecord>> UpsertLogs(
        IEnumerable<LogRecord> logRecords
        , bool debug_mode = false
    )
    {
        string table_name = "logs";
        var schema = await GetTableSchema(table_name);

        var query = CreateBulkInsertQuery(logRecords, table_name);

        // if (debug_mode)
        Console.WriteLine("full bulk insert query :>> " + query);
        logRecords.Count().Dump("total logs");

        string safe_path = Directory.GetCurrentDirectory().GoUp(2);
        Console.WriteLine("safely saving out of hot reload's reach to: >> " + safe_path);
        FS.SaveAs(new SaveAs(file_name: "insert_query.sql") { root_path = safe_path }, query);

        try
        {
            var connectionString = await GetConnectionString();
            using var connection = new MySqlConnection(connectionString);

            var results = connection
                .Query<LogRecord>(query
                    , commandType: CommandType.Text
                )
                .ToList();

            return results ?? new List<LogRecord>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            WriteLocalLogfile(e.ToString() + query);
            throw;
        }
    }

    private string CreateBulkInsertQuery(IEnumerable<LogRecord> logRecords, string table_name)
    {
        
        
        var props = typeof(LogRecord).GetProperties().OrderBy(p => p.Name).ToList();
        var names = props.Select(p => p.Name).ToList();
        names.Dump("ordered names: ");
        //1. build aliased insert values query:
        // string insert_aliases = CreateAliases(names);
        string insert_names_subquery = CreateInsertNames(names);
        Console.WriteLine("insert_names_subquery :>> " + insert_names_subquery);

        string insert_into_subquery = $"insert into {table_name} {insert_names_subquery}";

        // 2. Hydrate insert values for each prop:

        int index = 0;
        int end = logRecords.Count();

        var insert_values_subquery = logRecords
            .Aggregate(new StringBuilder(" VALUES ("), (builder, next) =>
            {
                var dictionary = next.ToDictionary();

                // builder.AppendLine(next.exception_message);
                int k = 0;
                foreach (var kvp in dictionary)
                {
                    var value = kvp.Value.ToString();
                    string name = kvp.Key;
                    var type = props.Find(p => p.Name.Equals(name))?.PropertyType;
                    // Console.WriteLine("name: " + name);
                    // Console.WriteLine("type :>> " + type);
                    // Console.WriteLine("value: " + value);

                    string delimiter = k == 0 ? " " : ", ";

                    if (value == null)
                        builder.AppendLine($"{delimiter}null");
                    //         Regex.Replace(insert_aliases, $@"${name}", "null");
                    if (type == typeof(bool))
                        // Regex.Replace(insert_aliases, $@"${name}", $", '{value.ToString()?.ToLower()}'");
                        builder.AppendLine($"{delimiter}{value.ToString()?.ToLower()}");
                    if (type == typeof(string))
                    {
                        string text = value.ToString();
                        text = text.Replace("'", "&apos;"); // normalize all single quotes.
                        // Regex.Replace(insert_aliases, $@"${name}", $"( '{value}'");
                        builder.AppendLine($"{delimiter}'{text}'");
                    }
                    //     builder.AppendLine(insert_aliases + ",");

                    k++;
                }

                index++;

                if (index != end)
                    builder.AppendLine("),");
                else
                    builder.AppendLine(")");
                return builder;
            }).ToString();

        // 4. Combine.
        string query = StringBuilderExtensions.RemoveFromEnd(new StringBuilder()
                .AppendLine(insert_into_subquery)
                .AppendLine(insert_values_subquery), 2) // remove last comma
            .Append(";") // adds the delimiter for mysql
            .ToString();

        return query;
    }

    private string CreateInsertNames(IEnumerable<string> names)
    {
        var sb = new StringBuilder("(");
        foreach ((var property_name, var index)in names.WithIndex())
        {
            if (index == 0)
                sb.AppendLine($"{property_name}"); // no comma.
            else
                sb.AppendLine($", {property_name}"); // no comma.
        }

        sb.RemoveFromEnd(1); // remove last comma
        sb.AppendLine(")"); // ending parenthesis

        return sb.ToString();
    }


    private string CreateAliases(IEnumerable<string> names)
    {
        var sb = new StringBuilder("(");
        foreach ((var property_name, var index)in names.WithIndex())
        {
            if (index == 0)
            {
                sb.AppendLine($"${property_name}");
            } // no comma.

            else if (index != 0)
            {
                sb.AppendLine($", ${property_name}"); // no comma.
            }
        }

        sb.RemoveFromEnd(1); // remove last comma
        sb.AppendLine(")"); // ending parenthesis

        return sb.ToString();
    }

    public string WriteLocalLogfile(string content)
    {
        var cwd = Environment.CurrentDirectory;
        var filepath = Path.Combine(cwd, "Admin.log");
        Console.WriteLine("writing to :>> " + filepath);
        System.IO.File.WriteAllText(filepath, content);
        return filepath;
    }
}

/// <summary>
/// See: https://stackoverflow.com/questions/33559437/how-to-get-table-structure-and-its-data-through-mysql-query
/// </summary>
public class SchemaInfo
{
    public string field { get; set; } = string.Empty;
    public string type { get; set; } = string.Empty;
    public string Null { get; set; } = string.Empty;
    public string key { get; set; } = string.Empty;
    public string Default { get; set; } = string.Empty;
    public string extra { get; set; } = string.Empty;
}

public class CreateTableInfo
{
    public string Create_Table { get; set; } = string.Empty;
    public string Table { get; set; } = string.Empty;
}

public interface IGlobalLoggingService
{
    Task<List<LogRecord>> GetAllLogs();
    Task<List<LogRecord>> BulkUpsertLogs(List<LogRecord> logRecords);

    Task<List<LogRecord>> UpsertLogs(
        IEnumerable<LogRecord> logRecords
        , bool debug_mode = false
    );
}