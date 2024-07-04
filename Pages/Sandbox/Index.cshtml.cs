using System.Net;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Sandbox;

public class Index : PageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetHtmlFromAPI(string url)
    {
        if (url.IsEmpty()) return Content("no url provided.");
        var content = await CallUrl(url);
        return Content(content);
    }


    private static async Task<string> CallUrl(string fullUrl)
    {
        HttpClient client = new HttpClient();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
        client.DefaultRequestHeaders.Accept.Clear();
        var response = client.GetStringAsync(fullUrl);
        return await response;
    }
}