using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Bogus;
using Dapper;
using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.RazorHAT.Services;
using CodeMechanic.Sqlc;
using CodeMechanic.Types;
using evantage.Models;
using MySqlConnector;

namespace evantage.Services;

public class NotesService : INotesService
{
    private readonly EmbeddedResourceService embeds;

    public NotesService(IEmbeddedResourceQuery embed_service)
    {
        embeds = (EmbeddedResourceService?)embed_service;
    }

    public async Task<int> GetCount()
    {
        string sql = """SELECT count(id) FROM notes""";
        using var connection = CreateConnection();
        var count = await connection.ExecuteAsync(sql);
        return count;
    }

    public async Task<List<Note>> GetAll()
    {
        string sql = """SELECT * FROM notes""";
        using var connection = CreateConnection();
        var records = await connection.QueryAsync<Note>(sql);
        return records.ToList();
    }

    public async Task<List<Note>> Search(Note search)
    {
        // string sql = embeds.GetFileContents<NotesService>("search_notes.sql");
        string sql = ReadResourceFile("search_notes.sql");
        Console.WriteLine(sql);
        // search.Dump(nameof(search));

        using var connection = CreateConnection();

        var records = await connection.QueryAsync<Note>(sql, search);
        return records.ToList();
    }

    private MySqlConnection CreateConnection()
    {
        string connectionstring = GetConnectionString();
        return new MySqlConnection(connectionstring);
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

    public Task<Note> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<int> Create(params Note[] records)
    {
        records.Dump($"creating {records.Length} notes");
        string sql = embeds.GetFileContents<NotesService>("create_note.sql");
        // https://regex101.com/r/XyhgkI/1
        // string full =
        //     @"(?<insert_clause>insert\s*into\s\w+\s*\([\w,\s*]+\))\s*(?<values_clause>values\s*\([@\w,\s]+\)\s*\;?)$";
        string values_clause = @"(?<values_clause>values\s*\([@\w,\s]+\)\s*\;?)$";
        // var values_regex = new Regex(values_clause,
        //     RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        string updated_sql = Regex.Replace(sql, values_clause, "",
            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        Console.WriteLine($"without values clause : >> {updated_sql}");

        string bulk_sql = records
                .ToList()
                .Aggregate(new StringBuilder("values ")
                        .Prepend(updated_sql)
                    // .Prepend("begin transaction; \n")
                    , (builder, note) =>
                    {
                        builder.Append("(");
                        builder.Append($"'{note.Id.EscapeSingleQuotes()}', ");
                        builder.Append($"{note.Name}, ");
                        builder.Append($"'{note.Description}', ");
                        builder.Append($"'{note.Created}'");
                        builder.AppendLine("),");
                        return builder; //.RemoveFromEnd(2);
                    })
                .RemoveFromEnd(2)
                // .AppendLine(";\n commit;")
                .ToString()
            // .EscapeSingleQuotes()
            ;
        int record_count = 0;
        string message = $"-- bulk sql for {records.Length} records :>> \n" + bulk_sql;
        // new LocalLoggerService().WriteToFile<NotesService>("Notes service", message);
        Console.WriteLine(message);

        await using (var connection = CreateConnection())
        {
            connection.Open();
            await using (var transaction = connection.BeginTransaction())
            {
                var command = connection.CreateCommand();
                command.CommandText = bulk_sql;
                record_count = await command.ExecuteNonQueryAsync();
                transaction.Commit();
            }
        }

        // var record_count = await connection.ExecuteAsync(bulk_sql, records);
        Console.WriteLine("RECORDS CREATED : " + record_count);
        return record_count;
    }

    public async Task Update(int id, Note model)
    {
        string sql = embeds.GetFileContents<NotesService>("update_note.sql");
        using var connection = CreateConnection();
        var records = await connection.ExecuteAsync(sql, model);
        Console.WriteLine($"Records changed: {records}");
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    private List<Note> CreateFakeRecords(int count = 10)
    {
        var index = 1;
        var faker = new Faker<Note>()
                .CustomInstantiator(f => new Note() { Id = index++.ToString() })
                .RuleFor(o => o.Created, f => f.Date.Recent(100))
                .RuleFor(o => o.LastModified, f => f.Date.Recent(30))
            ;
        var items = faker.Generate(count);
        return items;
    }

    /// <summary>
    /// https://khalidabuhakmeh.com/how-to-use-embedded-resources-in-dotnet
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    private string ReadResourceFile(string filename)
    {
        var thisAssembly = Assembly.GetExecutingAssembly();
        using (var stream = thisAssembly.GetManifestResourceStream(filename))
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}