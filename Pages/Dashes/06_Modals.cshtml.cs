using System.ComponentModel.DataAnnotations;
using CodeMechanic.Diagnostics;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Dashes
{
    public class Modal : PageModel
    {
        public IActionResult OnPostModal([FromForm] NewsletterSignup signup)
        {
            signup.Dump();
            // Note: You might want more validation
            if (!ModelState.IsValid)
            {
                // oh no, refresh the page
                Response.Htmx(h => h.Refresh());
                return Content("", "text/html");
            }

            // return Partial("_Modal", signup);
            return Content("Success!");
        }
    }

    public class NewsletterSignup
    {
        [EmailAddress, Required] public string? Email { get; set; } = string.Empty;
    }
}