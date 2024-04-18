using System.Collections.Generic;
using Newtonsoft.Json;


namespace CodeMechanic.Todoist;

public static class TodoistExtensions
{
    public static List<T> Deserialize<T>(this string line) // where T : class
    {
        return JsonConvert.DeserializeObject<List<T>>(line);
    }
}