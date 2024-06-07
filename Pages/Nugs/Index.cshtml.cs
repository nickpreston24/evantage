using CodeMechanic.Airtable;
using CodeMechanic.Diagnostics;
using evantage.Models.Csv;
using evantage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Nugs;

public class Index : PageModel
{
    private readonly INugsService nugs;

    public Index(INugsService nugs)
    {
        this.nugs = nugs;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetAllNugs()
    {
        var results = await nugs.GetRecordsFromCSV<Part>();
        Console.WriteLine(nameof(OnGetAllNugs));
        return Content("Hello from " + nameof(OnGetAllNugs));
    }

    public async Task<IActionResult> OnGetAllRounds()
    {
        return Content("Hello from " + nameof(OnGetAllRounds));
    }
}