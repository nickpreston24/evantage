using CodeMechanic.Diagnostics;
using Dapper;
using evantage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;

namespace evantage.Pages.Dashes;

public class Index : PageModel
{
    public List<Earnings> AllWeeks => all_weeks;
    public List<YoutubeVideo> Videos => videos;

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
        results.Dump("earnings ");
    }

    public async Task<IActionResult> OnPostInsertEarnings()
    {
        string query = """
            insert into weekly_earnings(phone_app,
                fillups_for_the_week,
                days_run_this_week)

            values ('DoorDash', 4, 6),
            ('DoorDash', 5, 6),
            ('DoorDash', 7, 6),
            ('DoorDash', 6, 5)
                ;
            """;

        string cs = SQLConnections.GetMySQLConnectionString();
        using var connection = new MySqlConnection(cs);
        // string insert_query =
        //     @"insert into logs (exception_message, exception_text, application_name) values (@exception_message, @exception_text, @application_name)";

        var results = await Dapper.SqlMapper
            .QueryAsync(connection, query,
                new
                {
                    // application_name,
                    // exception_text = message,
                    // exception_message = "INFO"
                });

        int affected = results.ToList().Count;

        Console.WriteLine($"logged {affected} log records.");
        return Content($"rows inserted: {affected}");
    }

    private List<YoutubeVideo> videos = new()
    {
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