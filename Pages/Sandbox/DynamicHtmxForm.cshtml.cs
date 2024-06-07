using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Sandbox;

[BindProperties]
public class DynamicHtmxForm : PageModel
{
    public Player player { get; set; } = new Player();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostHtmxForm()
    {
        Console.WriteLine(nameof(OnPostHtmxForm));
        return Content("Name: " + player.Name);
    }
}

public class Player
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double Height { get; set; }
}