using System.Runtime.Caching;

namespace CodeMechanic.Airtable;

public class AirtableServiceV2 : IAirtableServiceV2
{
    private readonly string projectDirectory;
    private readonly string env_path;
    private readonly string nugs_base_id;
    private readonly string nugs_api_key;
    public bool debug_mode { get; set; } = true;

    private readonly MemoryCache cache;

    public AirtableServiceV2()
    {
        projectDirectory = Directory
            .GetParent(Environment.CurrentDirectory)
            ?.Parent?.Parent?.FullName;

        nugs_base_id = Environment.GetEnvironmentVariable("NUGS_BASE_KEY");
        nugs_api_key = Environment.GetEnvironmentVariable("NUGS_PAT");

        Console.WriteLine(nugs_base_id);
        Console.WriteLine(nugs_api_key);
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
}