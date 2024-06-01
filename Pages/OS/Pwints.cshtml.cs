using System.Diagnostics;
using CodeMechanic.Diagnostics;
using CodeMechanic.FileSystem;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using evantage.Services;

namespace evantage.Pages.OS;

[ResponseCache(Duration = 1 /* second */)]
public class Pwints : PageModel
{
    public void OnGet()
    {
    }

    public ContentResult OnGetRandom()
    {
        // Console.WriteLine(nameof(OnGetRandom) + " called");
        return Content($"<span>{Number.Value}</span>", "text/html");
    }

    public async Task<IActionResult> OnGetGrepFiles(string file_mask, string root_directory, bool debug)
    {
        try
        {
            Stopwatch watch = Stopwatch.StartNew();

            Console.WriteLine("file_mask :>> " + file_mask);
            if (debug)
                Console.WriteLine(nameof(OnGetGrepFiles));

            string root = root_directory.NotEmpty() ? root_directory : Directory.GetCurrentDirectory();
            Console.WriteLine(root);

            var found_files = new Grepper()
                {
                    RootPath = root,
                    FileSearchMask = file_mask.IsEmpty() ? "*.*" : file_mask,
                    Recursive = true
                }
                .GetFileNames()
                .Where(dirname => !dirname.Contains("node_modules")
                                  && !dirname.Contains("wwwroot")
                )
                .ToList();

            watch.Stop();
            var time = watch.ElapsedMilliseconds;

            found_files.Dump(nameof(found_files));

            return Partial("_DriveFiles", found_files);
            // return Content(
            //     $"<p class='text-lg text-accent'>total {file_mask} found:  {found_files.Count} in folder '{root}' .  Took {time} milliseconds.</p>");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // throw;
            return Partial("_Alert", e.Message);
        }
    }
}

public static class IEnumerableExtensions
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index));
    }
}