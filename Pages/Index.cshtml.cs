using CodeMechanic.Diagnostics;
using CodeMechanic.FileSystem;
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
    private IWebHostEnvironment _environment;

    public string Query { get; set; } = string.Empty;
    [BindProperty] public IFormFile Upload { get; set; }

    public IndexModel(ILogger<IndexModel> logger
        , IDownloadImages image_downloader
        , IWebHostEnvironment environment)
    {
        _environment = environment;

        _logger = logger;
        imageDownloader = image_downloader;
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

    public async Task OnPostAsync()
    {
        if (Upload == null || Upload.FileName.IsEmpty()) return;
        string save_dir = Path.Combine(_environment.ContentRootPath, "uploads");
        FS.MakeDir(save_dir);
        var save_path = Path.Combine(save_dir, Upload.FileName);
        Console.WriteLine($"Saving file to '{save_path}'");
        await using var fileStream = new FileStream(save_path, FileMode.Create);
        await Upload.CopyToAsync(fileStream);
    }

    private static List<Lead> MakeSampleLeads()
    {
        int max_leads = 10;
        var leads = Enumerable.Range(1, max_leads)
            .Select(index =>
                new Lead()
                {
                    Index = index,
                    CompanyName = "Acme " + index,
                    CustomerName = "Wile E. Coyote"
                }
            )
            .ToList();

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