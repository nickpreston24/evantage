using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeMechanic.FileSystem;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Leads.Import;

[BindProperties(SupportsGet = true)]
public class ImportCSV : PageModel
{
    public List<string> CsvFileNames { get; set; } = new();
    [BindProperty] public IFormFile Upload { get; set; }
    private IWebHostEnvironment _environment;

    public ImportCSV(
        IWebHostEnvironment environment
    )
    {
        _environment = environment;
    }

    public void OnGet()
    {
        CsvFileNames = FindLocalCSVs();
        Console.WriteLine($"found {CsvFileNames.Count} csv files");
    }

    public async Task<IActionResult> OnGetCountCSVFiles()
    {
        var files = FindLocalCSVs();
        return Content($"<span>{files.Count}</span>");
    }

    public async Task OnPostAsync()
    {
        if (Upload == null || Upload.FileName.IsEmpty()) return;
        string save_dir = Path.Combine(_environment.ContentRootPath, "uploads");
        FS.MakeDir(save_dir);
        var save_path = Path.Combine(save_dir, Upload.FileName);
        Console.WriteLine($"Saving file to '{save_path}'");
        await using var fileStream = new FileStream(save_path, FileMode.Create);
        await Upload.CopyToAsync(fileStream);
    }

    private static List<string> FindLocalCSVs()
    {
        string cwd = Directory.GetCurrentDirectory();
        string csv_uploads_folder = "uploads";
        string csv_folder_path = Path.Combine(cwd, csv_uploads_folder);
        var grepper = new Grepper()
        {
            RootPath = csv_folder_path,
            FileSearchMask = "*.csv"
        };

        var files = grepper.GetFileNames().ToList();
        return files;
    }
}