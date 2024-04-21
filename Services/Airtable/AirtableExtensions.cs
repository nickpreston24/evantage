using Newtonsoft.Json;

namespace CodeMechanic.Airtable;

public static class AirtableExtensions
{
    public static List<T> Deserialize<T>(this string line) // where T : class
    {
        return JsonConvert.DeserializeObject<List<T>>(line);
    }
}