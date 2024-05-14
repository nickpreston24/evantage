using CodeMechanic.FileSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Logs;

[BindProperties(SupportsGet = true)]
public class Index : PageModel
{
    private IGlobalLoggingService logging_svc;

    // private static string connectionString;
    // public string SelectedProgram { get; set; } = "code-insiders";
    public Index(IGlobalLoggingService globalLoggingService)
    {
        logging_svc = globalLoggingService;
    }

    public void OnGet()
    {
    }

    public async Task OnGetOpenInExplorer(string filepath, string program)
    {
        Console.WriteLine("opening file :>> " + filepath);
        FS.OpenWith(filepath, program);
        // if (System.IO.File.Exists(filepath))
        // {
        //     Process.Start(program, filepath);
        // }
    }

    public async Task<IActionResult> OnGetLocalLogFiles()
    {
        try
        {
            var results = GetLocalLogs();
            // return Content(message);
            return Partial("_FileSystemLogsTable", results);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Partial("_Alert", e);
            // throw;
        }
    }

    private static List<Grepper.GrepResult> GetLocalLogs()
    {
        string mask = @"*.log";
        string cwd = Directory.GetCurrentDirectory().GoUp(0);
        Console.WriteLine("looking for logs in :>>" + cwd);
        var grepper = new Grepper()
        {
            RootPath = cwd, FileSearchMask = mask, Recursive = true, FileNamePattern = ".*.log",
            FileSearchLinePattern = ".*"
        };

        var results = grepper.GetMatchingFiles().ToList();
        int count = results.Count;
        string message = $"Found {count} files";
        Console.WriteLine(message);
        return results;
    }

    public async Task<IActionResult> OnGetAllLogs()
    {
        var results = await logging_svc.GetAllLogs();

        // return Content($"{count}");
        return Partial("_LogsTable", results);
    }
}