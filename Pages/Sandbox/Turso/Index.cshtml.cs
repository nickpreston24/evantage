using System.Net.Http.Headers;
using System.Text;
using CodeMechanic.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Libsql.Client;

namespace evantage.Pages.Sandbox.Turso;

public class Index : PageModel
{
    private static string turso_auth_token;
    private static string turso_db_url;

    public void OnGet()
    {
        turso_db_url = Environment.GetEnvironmentVariable("TURSO_DATABASE_URL");
        turso_auth_token = Environment.GetEnvironmentVariable("TURSO_AUTH_TOKEN");
    }

    public async Task<IActionResult> OnGetConnectToTurso()
    {
        // TODO: Neither of these are working.
        
        // await RunOverHttp();

        // await RunOverSDK();

        return Content("success!");
    }

    private async Task RunOverSDK()
    {
        // Create an in-memory database.
        var dbClient = await DatabaseClient.Create(opts =>
            // using (var dbClient = DatabaseClient.Create(opts =>
        {
            opts.Url = turso_db_url;
            opts.AuthToken = turso_auth_token;
        });

        // {
        var response = await dbClient.Execute("select * from parts");
        response.Dump("whatever this is..");
        // }


        // dbClient.Dispose();
    }

    private async Task RunOverHttp()
    {
        Console.WriteLine("url " + turso_db_url);
        Console.WriteLine("auth " + turso_auth_token);

        // var objAsJson = JsonConvert.SerializeObject(new Part() { Name = "The Liberator" });
/*  requests: [
      { type: "execute", stmt: { sql: "SELECT * FROM users" } },
      { type: "close" },
    ],*/


// setup post

        var objAsJson = JsonConvert.SerializeObject(new TursoRequests()
        {
            requests = new List<TursoRequest>()
            {
                new TursoRequest() { type = "execute", stmt = new Statement() { sql = "SELECT * FROM parts" } },
                new TursoRequest() { type = "close" },
            }
        });

        Console.WriteLine(" objAsJson : " + objAsJson);
        var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");
        using HttpClient http_client = new HttpClient();
        http_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", turso_auth_token);

        // run
        var response = await http_client.PostAsync(turso_db_url, content);

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine("response from turso:\n" + json);
    }
    /*
     * Sample from docs: https://docs.turso.tech/sdk/http/quickstart
      fetch(url, {
  method: "POST",
  headers: {
    Authorization: `Bearer ${authToken}`,
    "Content-Type": "application/json",
  },
  body: JSON.stringify({
    requests: [
      { type: "execute", stmt: { sql: "SELECT * FROM users" } },
      { type: "close" },
    ],
  }),
})
  .then((res) => res.json())
  .then((data) => console.log(data))
  .catch((err) => console.log(err));
     */
}

public class TursoRequests
{
    public List<TursoRequest> requests { get; set; } = new List<TursoRequest>() { new TursoRequest() };
}

public class TursoRequest
{
    public string type { get; set; } = "execute";

    public Statement stmt { get; set; } = new();
}

public class Statement
{
    public string sql { get; set; }
}