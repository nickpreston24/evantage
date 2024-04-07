using CodeMechanic.Types;
using evantage.Models;
using evantage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages;

[BindProperties]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IDownloadImages imageDownloader;

    public IndexModel(
        ILogger<IndexModel> logger
        , IDownloadImages image_downloader
    )
    {
        _logger = logger;
        imageDownloader = image_downloader;
    }

    public Commissions Commission { get; set; } = new();

    public void OnGet()
    {
        // sample:
        MakeSampleCommissions();

        // DownloadSamplePicsumImages();
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