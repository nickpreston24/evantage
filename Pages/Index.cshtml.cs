using evantage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages;

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
    }

    public async Task<IActionResult> OnGetCommissionsCalculations()
    {
        double total = 0;

        return Partial("_CommissionCalculator", new Commissions());
    }
}