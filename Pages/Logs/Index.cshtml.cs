using System.Data;
using System.Diagnostics;
using System.Text;
using CodeMechanic.Async;
using CodeMechanic.Types;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;

namespace evantage.Pages.Logs;

public class Index : PageModel
{
    private static string connectionString;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetAllLogs()
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
        string connectionstring = GetConnectionString();
        using var connection = new MySqlConnection(connectionstring);

        var results = connection
            .Query<LogRecord>(query
                , commandType: CommandType.Text
            )
            .ToList();

        int count = results.Count;

        // return Content($"{count}");
        return Partial("_LogsTable", results);
    }

    private static string GetConnectionString()
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

    private async Task<List<LogRecord>> BulkUpsertLogs(List<LogRecord> logRecords)
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

    private async Task<List<LogRecord>> UpsertLogs(
        IEnumerable<LogRecord> logRecords
        , bool debug_mode = false
    )
    {
        var insert_values = logRecords
            .Aggregate(new StringBuilder(), (builder, next) =>
            {
                builder
                    .AppendLine($"( '{next.application_name}'")
                    .AppendLine($", '{next.database_name}'   ")
                    .AppendLine($", '{next.exception_text}'  ")
                    .AppendLine($", '{next.breadcrumb}'      ")
                    .AppendLine($", '{next.issue_url}'       ")
                    .AppendLine($", '{next.created_by}'      ")
                    .AppendLine($", '{next.modified_by}'     ")
                    .AppendLine($", null")
                    .AppendLine($", null")
                    .AppendLine($", '{next.is_archived}'     ")
                    .AppendLine($", '{next.is_deleted}'      ")
                    .AppendLine($", '{next.is_enabled}'      ")
                    .AppendLine($")")
                    .ToString()
                    .Trim();
                builder.Append(",");
                return builder;
            }).ToString();

        if (debug_mode) Console.WriteLine("values query :>> " + insert_values);

        string insert_begin = """ 
                        insert into logs 
                        ( 
                            table_name
                         , database_name
                         , exception_text
                         , breadcrumb
                         , issue_url
                         , created_by
                         , modified_by
                         , created_at
                         , modified_at
                         , is_deleted
                         , is_archived
                         , is_enabled
                     )
                    values
                    """;

        var query = StringBuilderExtensions.RemoveFromEnd(new StringBuilder()
                .AppendLine(insert_begin)
                .AppendLine(insert_values), 2) // remove last comma
            .Append(";") // adds the delimiter for mysql
            .ToString();

        if (debug_mode) Console.WriteLine("full query :>> " + query);

        try
        {
            var connectionString = GetConnectionString();
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

    private static void WriteLocalLogfile(string content)
    {
        var cwd = Environment.CurrentDirectory;
        string filepath = Path.Combine(cwd, "Admin.log");
        Console.WriteLine("writing to :>> " + filepath);
        System.IO.File.WriteAllText(filepath, content);
    }
}

public sealed class LogRecord
{
    public string id { get; set; } = string.Empty;
    public string exception_text { get; set; } = string.Empty;
    public string exception_message { get; set; } = "OOOPS!"; // = string.Empty;

    public string exception_severity { get; set; } = "HIGH";

    public string sql_parameters { get; set; } = string.Empty;
    public string payload { get; set; } = string.Empty;
    public string diff { get; set; } = "{}";
    public string operation_name { get; set; } = string.Empty;
    public string breadcrumb { get; set; } = string.Empty;
    public string table_name { get; set; } = string.Empty;
    public string server_name { get; set; } = string.Empty;
    public string database_name { get; set; } = "railway"; // = string.Empty;
    public string application_name { get; set; } = string.Empty;
    public string modified_by { get; set; } = string.Empty;
    public string created_by { get; set; } = string.Empty;
    public DateTime modified_at { get; set; }
    public DateTime created_at { get; set; }

    public string commit_url { get; set; } = string.Empty;
    public string issue_url { get; set; } = string.Empty;
    public bool is_deleted { get; set; }
    public bool is_archived { get; set; }
    public bool is_enabled { get; set; }
}