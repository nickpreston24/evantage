using System.ComponentModel.DataAnnotations;
using CodeMechanic.Diagnostics;
using Dapper;
using evantage.Models;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;

namespace evantage.Pages.Dashes;

public class Index : PageModel
{
    public List<Earnings> AllWeeks => all_weeks;
    public List<YoutubeVideo> Videos => videos;

    [BindProperty, Required] public string? Name { get; set; } = string.Empty;

    [BindProperty, Required] public int Age { get; set; } = -1;

    public async Task<IActionResult> OnPostEarnings()
    {
        // see the loading spinner (remove for production)
        await Task.Delay(TimeSpan.FromSeconds(1));
        // handle Htmx request
        return Request.IsHtmx()
            ? Partial("_EarningsForm", this)
            : Page();
    }

    public async Task<IActionResult> OnPostYoutube()
    {
        // see the loading spinner (remove for production)
        await Task.Delay(TimeSpan.FromSeconds(1));
        // handle Htmx request
        return Request.IsHtmx()
            ? Partial("_YoutubeForm", this)
            : Page();
    }


    public async Task OnGet()
    {
        string query = """
            select phone_app,
                   fillups_for_the_week,
                   days_run_this_week
            from weekly_earnings;
        """;

        string cs = SQLConnections.GetMySQLConnectionString();
        using var connection = new MySqlConnection(cs);

        var results = await connection.QueryAsync<Earnings>(query);
        // results.Dump("earnings ");
        all_weeks = results.ToList();
    }


    public IActionResult OnPostInsertEarnings([FromForm] NewsletterSignup signup)
    {
        signup.Dump(nameof(signup));
        // Note: You might want more validation
        if (!ModelState.IsValid)
        {
            // oh no, refresh the page
            Response.Htmx(h => h.Refresh());
            return Content("", "text/html");
        }

        int affected = -1;
        return Content($"rows inserted: {affected}");
    }


    public class NewsletterSignup
    {
        [EmailAddress, Required] public string? Email { get; set; } = string.Empty;
        [EmailAddress, Required] public string? Password { get; set; } = string.Empty;
    }

//     public async Task<IActionResult> OnPostInsertEarnings()
//     {
//         Console.WriteLine(nameof(OnPostInsertEarnings));
//         string query = """
//             insert into weekly_earnings(phone_app,
//                 fillups_for_the_week,
//                 days_run_this_week)
//
//             values ('DoorDash', 4, 6),
//             ('DoorDash', 5, 6),
//             ('DoorDash', 7, 6),
//             ('DoorDash', 6, 5)
//                 ;
//             """;
//
//         string cs = SQLConnections.GetMySQLConnectionString();
//         using var connection = new MySqlConnection(cs);
//         // string insert_query =
//         //     @"insert into logs (exception_message, exception_text, application_name) values (@exception_message, @exception_text, @application_name)";
//
//         var results = await Dapper.SqlMapper
//             .QueryAsync(connection, query,
//                 new
//                 {
//                     // application_name,
//                     // exception_text = message,
//                     // exception_message = "INFO"
//                 });
//
//         int affected = results.ToList().Count;
//
//         Console.WriteLine($"logged {affected} log records.");
//         return Content($"rows inserted: {affected}");
//     }

    private List<YoutubeVideo> videos = new()
    {
        new YoutubeVideo() { Url = "https://www.youtube.com/watch?v=Yxy7LZihwgc", },
        new YoutubeVideo() { Url = "https://www.youtube.com/watch?v=Q7COkq2iZcY" },
        new YoutubeVideo() { Url = "https://www.youtube.com/watch?v=hF0YQPPcx-w", Status = "Watched" },
        new YoutubeVideo()
            { Url = "https://www.youtube.com/watch?v=szpQppp0svE&list=TLPQMDYwNjIwMjTrs7eH0o9FTg&index=2&pp=gAQBiAQB" }
    };

    private List<Earnings> all_weeks = new()
    {
        new Earnings() { average_gas_tank_cost = 65.00 },
        new Earnings() { average_gas_tank_cost = 70.00 },
        new Earnings() { average_gas_tank_cost = 68.50 },
    };
}