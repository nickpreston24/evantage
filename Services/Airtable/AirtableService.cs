using System.Net.Http.Headers;
using System.Runtime.Caching;
using CodeMechanic.Diagnostics;
using CodeMechanic.Types;
using Newtonsoft.Json.Linq;

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

        base_id = Environment.GetEnvironmentVariable("AIRTABLE_SALES_SPY_BASE_ID");
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

        // Add data to the cache with an expiration time of 5 time_value
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
        if (string.IsNullOrEmpty(search.table_name))
            search.table_name = typeof(T).Name + "s";

        if (search.airtable_pat.IsEmpty()) throw new ArgumentNullException("airtable_pat");
        if (search.base_id.IsEmpty()) throw new ArgumentNullException("base_id");

        if (debug) Console.WriteLine($"base id:>>{base_id}");
        if (debug) Console.WriteLine("pat:>>" + search.airtable_pat);
        if (debug) search.Dump(nameof(search));

        using HttpClient http_client = new HttpClient();
        http_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", search.airtable_pat);
        // http_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", api_key);
        string url = search
                // .With(s =>
                // {
                //     // s.base_id = base_id;
                //     // s.table_name = table_name;
                // })
                .AsQuery()
            // .Dump("Full query:>>")
            ;
        if (debug) Console.WriteLine("url:>> " + url);
        var response = await http_client.GetAsync(url);

        if (debug) response.Dump("raw response");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        if (debug) Console.WriteLine("Here's the raw JSON:");
        Console.WriteLine(json);

        // var list = new RecordList<T>(json);
        // var (_, records) = new RecordList<T>(json);

        var records = GetRecordsFromJson<T>(json);
        return records;
    }

    private List<T> GetRecordsFromJson<T>(string json = "{}")
    {
        JObject search = JObject.Parse(json);

        // get JSON result objects into a list
        List<JToken> tokens = search["records"]
            .Children()
            // .Dump("children")
            ["fields"]
            // .Dump("fields children")
            .ToList();


        // serialize JSON results into .NET objects
        var records = new List<T>();

        foreach (JToken token in tokens)
        {
            token.Dump(nameof(token));
            // JToken.ToObject is a helper method that uses JsonSerializer internally
            T instance = token.ToObject<T>();
            instance.Dump(nameof(instance));
            records.Add(instance);
        }

        return records;
    }
}