using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Nugs;

public class Index : PageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetAllNugs()
    {
        Console.WriteLine(nameof(OnGetAllNugs));
        return Content("Hello from " + nameof(OnGetAllNugs));
    }
}