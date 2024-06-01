using evantage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Sandbox;

/// <summary>
/// We will be receiving events from the ServerEventsWorker service
/// about the need to update our UI. We've registered the worker
/// as a hosted service in our startup file (Program.cs).
/// <seealso cref="ServerEventsWorker"/>
/// </summary>  
[ResponseCache(Duration = 1 /* second */)]
public class SSE : PageModel
{
    public void OnGet()
    {
    }

    public ContentResult OnGetRandom()
    {
        return Content($"<span>{Number.Value}</span>", "text/html");
    }
}