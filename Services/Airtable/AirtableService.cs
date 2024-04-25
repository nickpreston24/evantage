using System.Net.Http.Headers;
using System.Runtime.Caching;
using CodeMechanic.Diagnostics;
using CodeMechanic.RazorHAT;
using CodeMechanic.Types;

namespace CodeMechanic.Airtable;

public class AirtableServiceV2 : IAirtableServiceV2
{
    private readonly string projectDirectory;
    private readonly string env_path;
    private readonly string base_id;
    private readonly string api_key;
    public bool debug_mode { get; set; } = true;

    private readonly MemoryCache cache;
    // private readonly HttpClient http_client;

    public AirtableServiceV2(
        // HttpClient client
        // , string base_id = ""
        // , string personal_access_token = ""
    )
    {
        projectDirectory = Directory
            .GetParent(Environment.CurrentDirectory)
            ?.Parent?.Parent?.FullName;

        base_id = Environment.GetEnvironmentVariable("AIRTABLE_LEADS_BASE");
        api_key = Environment.GetEnvironmentVariable("AIRTABLE_API_KEY");

        Console.WriteLine(base_id);
        Console.WriteLine(api_key);


        // TODO: use the httpclientfactory pattern later.
        // http_client = client;
        // http_client.DefaultRequestHeaders.Authorization =
        //     new AuthenticationHeaderValue("Bearer", personal_access_token);

        // set up a cahce for json:
        // Create a MemoryCache instance
        cache = MemoryCache.Default;

        // Define cache key and data
        string cacheKey = "FullName";
        string cachedData = "Nick Preston";

        // Add data to the cache with an expiration time of 5 minutes
        CacheItemPolicy cachePolicy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };

        cache.Add(cacheKey, cachedData, cachePolicy);
    }

    public async Task<List<T>> SearchRecords<T>(
        AirtableSearchV2 search
        , bool debug = false)
    {
        var (
            table_name,
            offset,
            fields,
            filterByFormula,
            maxRecords,
            pageSize,
            sort,
            view,
            cellFormat,
            timeZone,
            userLocale,
            returnFieldsByFieldId
            ) = search /*.Dump("search")*/;

        if (string.IsNullOrEmpty(table_name))
            table_name = typeof(T).Name + "s";

        using HttpClient http_client = new HttpClient();
        var response = await http_client
            .GetAsync(search
                .With(s =>
                {
                    s.base_id = base_id;
                    s.table_name = table_name;
                })
                .AsQuery()
            );
        if (debug)
            response.Dump("raw response");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        // Console.WriteLine("Here's the raw JSON:");
        // Console.WriteLine(json);

        var list = new RecordList<T>(json);
        var (_, records) = new RecordList<T>(json);
        return records;
    }
}