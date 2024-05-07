using CodeMechanic.Diagnostics;
using evantage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Sandbox.ThreeJS;

public class Index : PageModel
{
    private readonly ICopyPastaService copy_pasta;

    public Index(ICopyPastaService copypasta)
    {
        this.copy_pasta = copypasta;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetCloneMyCode(
        string source_dir = "nugs-net6",
        string target_dir = "/Pages/Shared")
    {
        Console.WriteLine(nameof(OnGetCloneMyCode));
        var options = new CopyOptions()
        {
            source_dir = source_dir, target_dir = target_dir, directory_regex_pattern = "Pages",
            FileSearchMask = "*.cshtml",
        };

        await foreach (var result in copy_pasta.CopyAll(options))
        {
            result.Dump("async GREP Result");
            // do something
        }

        return Content("Done.");
    }
}