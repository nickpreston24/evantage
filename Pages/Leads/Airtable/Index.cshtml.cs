using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Leads.Airtable;

[BindProperties(SupportsGet = true)]
public class Index : PageModel
{
    public void OnGet(string viewname = "")
    {
        this.ViewName = viewname;
        Console.WriteLine(ViewName);
    }

    public string ViewName { get; set; } = string.Empty;
}