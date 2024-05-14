using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMechanic.Advanced.Regex;
using CodeMechanic.Airtable;
using CodeMechanic.Curl;
using CodeMechanic.Diagnostics;
using CodeMechanic.Types;
using evantage.Models;
using evantage.Services;
using Htmx;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using AirtableSearchV2 = CodeMechanic.Airtable.AirtableSearchV2;

namespace evantage.Pages;

[BindProperties(SupportsGet = true)]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IDownloadImages imageDownloader;
    private readonly IAirtableServiceV2 airtable_service;
    private readonly string base_id;
    private readonly string airtable_pat;

    public string Query { get; set; } = string.Empty;


    public IndexModel(ILogger<IndexModel> logger
        , IDownloadImages image_downloader
        , IAirtableServiceV2 airtable_svc
    )
    {
        _logger = logger;
        imageDownloader = image_downloader;
        airtable_service = airtable_svc;
        base_id = Environment.GetEnvironmentVariable("AIRTABLE_SALES_SPY_BASE_ID") ?? "<base_id>";
        airtable_pat = Environment.GetEnvironmentVariable("AIRTABLE_API_KEY") ?? "<pat>";
        Console.WriteLine(airtable_pat);
    }

    public Commissions Commission { get; set; } = new();

    private static List<Lead> CurrentLeads { get; set; } = new(); //= MakeSampleLeads();

    // The filtered results from current leads
    public List<Lead> Results { get; set; } = new();


    public async Task<ActionResult> OnGet()
    {
        var from_airtable = await GetAllLeadsFromAirtable();

        Results = string.IsNullOrEmpty(Query)
            ? CurrentLeads
            // .Dump("current leads")
            : CurrentLeads
                .Where(lead => lead.ToString()
                    .Contains(Query, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (!Request.IsHtmx())
            return Page();

        Response.Htmx(headers =>
        {
            // we want to push the current url 
            // into the history
            headers.Push(Request.GetEncodedUrl());
        });


        return Partial("_Results", this);
    }

    public async Task<IActionResult> OnGetAllInteractions(string term = "")
    {
        string tablename = "Interactions";
        Console.WriteLine(nameof(OnGetAllInteractions));
        // throw new NotImplementedException(nameof(OnGetAllInteractions));

        // string url = $"https://api.airtable.com/v0/{base_id}/{tablename}?maxRecords=3&view=Calls%20%26%20meetings";
        // Console.WriteLine("url:>>" + url);
        var search = new AirtableSearchV2(base_id, tablename, 10) { };

        search.Dump("search", ignoreNulls: false);

        if (airtable_service == null)
            return Content("service is null");
        var results = await airtable_service
            .SearchRecords<Part>(search
                // .With(searchmod =>
                // {
                //     searchmod.filterByFormula = $"(FIND(\"{term}\", {{Name}}))";
                // })
                , debug: true
            );
        //
        // results.Dump(nameof(results));

        // return Partial("_LeadsTable", this);
        return Content("test");
    }

    public async Task<IActionResult> OnGetAllLeadSources()
    {
        throw new NotImplementedException(nameof(OnGetAllLeadSources));
        // return Partial("_LeadsTable", this);
    }

    public async Task<IActionResult> OnGetAllScripts()
    {
        string tablename = "Parts";
        Console.WriteLine(nameof(OnGetAllInteractions));
        var search = new AirtableSearchV2(base_id, tablename, 10) { };
        search.Dump("search", ignoreNulls: false);

        if (airtable_service == null)
            return Content("service is null");
        var results = await airtable_service
            .SearchRecords<Script>(search
                , debug: true
            );
        //
        results.Dump(nameof(results));

        // return Partial("_LeadsTable", this);
        return Content("test");
    }


    private static List<Lead> MakeSampleLeads()
    {
        var leads = new List<Lead>();

        int max_fake_leads = 1;
        var fake_leads = Enumerable.Range(1, max_fake_leads)
            .Select(index =>
                new Lead()
                {
                    Index = index,
                    CompanyName = "Acme " + index,
                    CustomerName = "Wile E. Coyote",
                    Role = CompanyRole.Unknown,
                }
            )
            .ToList();
        ;

        leads.AddRange(fake_leads);

        leads.Add(new Lead()
        {
            PhoneNumber = "(512) 993-0765",
            CustomerName = "Jacob Thomas Gumns",
            CompanyName = "Lone Wolf Leathers",
            Role = CompanyRole.Owner,
            Notes = new string[]
            {
                "Leathering is his hobby; 3pm CST, Mobile card scanner would be deal, Android user, tomorrow and Monday, Storefront, Open date is May 17th"
            }
        });

        return leads
            // .Dump("leads created")
            ;
    }

    public async Task<IActionResult> OnGetSearch()
    {
        Console.WriteLine(nameof(OnGetSearch));
        // return Content("You Searched: " + Query);
        Results = CurrentLeads.Where(lead => lead.CompanyName.Contains(Query)).ToList();
        Results.Count.Dump("# of results");
        CurrentLeads.Count.Dump("# of results");
        return Partial("_LeadsTable", this);
    }

    private void DownloadSamplePicsumImages()
    {
        imageDownloader.DownloadImages();
    }

    private void MakeSampleCommissions()
    {
        var lines = new List<Line>()
        {
            new Line() { InstallmentPlan = InstallmentPlan.NextUp },
            new Line() { InstallmentPlan = InstallmentPlan.NextUp }
        };
        Commission = new Commissions()
            .With(c => c.Lines.AddRange(lines));
    }

    public async Task<IActionResult> OnPostCommissionsCalculations()
    {
        double total = 0;

        return Partial("_CommissionCalculator", new Commissions());
    }


    private List<CurlOptions> GetClient(string curl)
    {
        var curlRegex = get_regex_from_curl(curl);
        var regex = CurlRegex.Find(curlRegex);
        // regex.Dump(nameof(regex));
        // Console.WriteLine(curl);

        var curl_options = curl.Extract<CurlOptions>(regex);

        // if (debug_mode)
        curl_options.Dump(nameof(curl_options));

        return curl_options;
    }

    private CurlRegex get_regex_from_curl(string curl)
    {
        if (Regex.IsMatch(curl, @"-X\s*(GET)"))
        {
            return CurlRegex.GET;
        }

        if (Regex.IsMatch(curl, @"-X\s*(POST)"))
        {
            return CurlRegex.POST;
        }

        return CurlRegex.HEADERS;
    }

    private async Task<List<Lead>> GetAllLeadsFromAirtable()
    {
        string curl = """
             curl "https://api.airtable.com/v0/BASE_ID/Interactions?maxRecords=3&view=Calls%20%26%20meetings" \
            -H "Authorization: Bearer YOUR_SECRET_API_TOKEN"
        """;

        string api_key = Environment.GetEnvironmentVariable("AIRTABLE_API_KEY") ?? "";
        string base_id = Environment.GetEnvironmentVariable("AIRTABLE_SALES_SPY_BASE_ID") ?? "";
        // Update the curl string to always have the most updated bearer token (and not a sample, like most tutorials)
        curl =
            Regex.Replace(
                curl
                , @"Bearer \$?\w+"
                , "Bearer " + api_key
            );
        // Update the baseId
        curl =
            Regex.Replace(
                curl
                , @"BASE_ID"
                , base_id
            );


        Console.WriteLine("Curl: " + curl);

        var options = curl
                .Extract<CurlOptions>(CurlRegex.GET.compiled)
                .Select(next => next
                    // .With(opts =>
                    // {
                    //     opts.execution_method = "GET";
                    //     // opts.bearer_token = api_key;
                    // })
                )
            ;
        // var options = GetClient(raw_get);
        // options.Dump("extracted_curl_request");
        return new List<Lead>();

        // Call

        var all_tasks = options
            .Select(curl_options => GetContentAsync(curl_options.uri, curl_options.bearer_token))
            .ToList();

        Console.WriteLine("Calling API...");
        Console.WriteLine("total tasks running :>> " + all_tasks.Count);

        var responses = await Task.WhenAll(all_tasks);
        Console.WriteLine("responses :>> ", responses.Length);

        return new List<Lead>();
    }

    public async Task<string> GetContentAsync(string uri, string bearer_token, bool debug = false)
    {
        using HttpClient http = new HttpClient();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer_token);
        var response = await http.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        if (debug)
            Console.WriteLine("content :>> " + content);
        return content;
    }
}

public class Part
{
    public string Name { get; set; } = "";
}

public class Interaction
{
    public string Notes { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Last_Modified { get; set; } = string.Empty;
    public string Created { get; set; } = string.Empty;

    /**
     *  "Notes": "Called Dr. Feelgood at 6pm Sat.",
                "Kind": "Sample",
                "Type": "Cold Call",
                "Interaction": "—Cold Call",
                "Last Modified": "2024-04-22T01:19:09.000Z",
                "Created": "2024-04-22T01:00:19.000Z"
     */
}

public class LeadSource
{
}

public class Script
{
    public string Name { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}