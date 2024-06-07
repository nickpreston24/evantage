using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.RazorHAT.Services;
using Dapper;
using evantage.Models;
using evantage.Models.Csv;
using Microsoft.Data.Sqlite;
using MySqlConnector;

public class NugsRepository : INugsRepository
{
    private readonly EmbeddedResourceService embeds;

    public NugsRepository(IEmbeddedResourceQuery embed_service)
    {
        embeds = (EmbeddedResourceService?)embed_service;
        // FindTables();
    }

    private void FindTable(string tablename)
    {
        using var connection = GetConnection();

        var tables = connection.Query<string>($"show table {tablename}");
        var tableNames = tables.Dump("tables found");
    }

    public async Task<IEnumerable<Part>> GetAll()
    {
        // string sql = embeds.GetFileContents<NugsRepository>("get_all_calendar_events.sql").Dump();
        string sql = """SELECT * FROM parts""";
        using var connection = GetConnection();
        var records = await connection.QueryAsync<Part>(sql);
        // records.Dump("records");
        return records;
    }

    public async Task<List<Part>> Search(Part search)
    {
        string sql = embeds.GetFileContents<NugsRepository>("search_parts.sql");
        // search.Dump(nameof(search));
        using var connection = GetConnection();
        var records = await connection.QueryAsync<Part>(sql, search);
        return records.ToList();
    }

    // private SqliteConnection CreateConnection() => new SqliteConnection("Data Source=Nugs.db");

    public Task<Part> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task Create(Part model)
    {
        string sql = embeds.GetFileContents<NugsRepository>("create-part.sql");
        model.Dump("creating part");
        using var connection = GetConnection();
        var records = await connection.ExecuteAsync(sql, model);
    }

    private static MySqlConnection GetConnection()
    {
        string cs = SQLConnections.GetMySQLConnectionString();
        return new MySqlConnection(cs);
    }

    public async Task Update(int id, Part model)
    {
        string sql = embeds.GetFileContents<NugsRepository>("update-part.sql");
        using var connection = GetConnection();
        var records = await connection.ExecuteAsync(sql, new
        {
            url = model.Url, name = model.Name, imageurl = model.ImageUrl, kind = model.Kind
        });

        Console.WriteLine($"Records changed: {records}");
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }
}