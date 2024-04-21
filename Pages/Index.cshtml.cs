using CodeMechanic.Diagnostics;
using CodeMechanic.RazorHAT.Services;
using CodeMechanic.Types;
using evantage.Models;
using evantage.Services;
using Htmx;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages;

[BindProperties(SupportsGet = true)]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IDownloadImages imageDownloader;
    private readonly IAirtableQueryingService airtable_queries;

    public string Query { get; set; } = string.Empty;
    

    public IndexModel(ILogger<IndexModel> logger
        , IDownloadImages image_downloader
        , IAirtableQueryingService airtable_queryer
        )
    {
      
        _logger = logger;
        imageDownloader = image_downloader;
        this.airtable_queries = airtable_queryer;
    }

    public Commissions Commission { get; set; } = new();

    private static List<Lead> CurrentLeads { get; set; } = MakeSampleLeads();

    // The filtered results from current leads
    public List<Lead> Results { get; set; } = new();


    public IActionResult OnGet()
    {
        Results = string.IsNullOrEmpty(Query)
            ? CurrentLeads.Dump("current leads")
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


    

    

    private static List<Lead> MakeSampleLeads()
    {
        int max_leads = 1;
        var leads = Enumerable.Range(1, max_leads)
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

        return leads.Dump("leads created");
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
}