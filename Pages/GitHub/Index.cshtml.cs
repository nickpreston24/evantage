using CodeMechanic.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace evantage.Pages.GitHub;

public class Index : PageModel
{
    public List<CsProjUpdate> Updates => updates;
    private static List<CsProjUpdate> updates { get; set; } = new();

    public void OnGet()
    {
        string url = HttpContext.Request.GetDisplayUrl();

        string cwd = Directory.GetCurrentDirectory();
        string file_path = Path.Combine(cwd, "Pages/GitHub", "sample_myget.json");
        string sample_myget_response = file_path;
        string packages_json = System.IO.File.ReadAllText(sample_myget_response);
        Console.WriteLine("lines: " + packages_json.Length);


        // TODO: fix this deser.
        // var packages = GetRecordsFromJson<MyGetPackage>();
        // Console.WriteLine("packages : " + packages.Count);
    }

    private List<T> GetRecordsFromJson<T>(string json = "{}")
    {
        JObject search = JObject.Parse(json);

        // get JSON result objects into a list
        List<JToken> tokens = search["packages"]
            .Children()
            // .Dump("children")
            // ["fields"]
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

    // public async Task<IActionResult> OnPostAsync()
    // {
    //     ...
    // }
}