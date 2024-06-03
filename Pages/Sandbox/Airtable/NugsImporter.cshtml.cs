using System.Globalization;
using CodeMechanic.RazorHAT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CsvHelper;

namespace evantage.Pages.Sandbox.Airtable;

public class NugsImporter : PageModel
{
    private readonly ICsvService csv_service;

    public NugsImporter(ICsvService csvService)
    {
        csv_service = csvService;
    }

    public void OnGet()
    {
        Console.WriteLine("hi");
    }

    //
    public async Task<IActionResult> OnGetPartsFromCSV()
    {
        try
        {
            Console.WriteLine(nameof(OnGetPartsFromCSV));
            string file_path = @"/home/nick/Downloads/Parts-Grid view.csv";
            using var reader = new StreamReader(file_path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<Part>().ToList();
            return Content(" total parts : " + records.Count);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

public class Part
{
    public string url { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
}

public class Loadout
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Inspiration { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public Part[] Medkit { get; set; } = Array.Empty<Part>().ToArray();
    public Part[] PrimaryArm { get; set; } = Array.Empty<Part>().ToArray();
    public Part[] Sidearm { get; set; } = Array.Empty<Part>().ToArray();
}

//
// public class LoadoutGenerator : ILoadoutGenerator
// {
// }
//
// public interface ILoadoutGenerator
// {
// }