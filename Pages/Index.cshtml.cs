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

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public Commissions Commission { get; set; } = new();

    public void OnGet()
    {
        // sample:
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