using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Leads.Airtable;

[BindProperties(SupportsGet = true)]
public class EditLead : PageModel
{
    public int Id { get; set; }

    public void OnGet(int id)
    {
        this.Id = id;
    }
}