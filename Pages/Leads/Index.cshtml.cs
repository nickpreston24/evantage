using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Leads;

public class Index : PageModel
{
    public void OnGet()
    {
    }
}

public record ImportRoute
{
    public string Link { get; set; } = string.Empty;
    public string Tip { get; set; } = string.Empty;
    public object Title { get; set; } = string.Empty;
}