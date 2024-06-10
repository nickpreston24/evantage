using System.Collections;
using CodeMechanic.Diagnostics;
using CodeMechanic.Types;
using evantage.Models;
using evantage.Models.Csv;
using evantage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Nugs;

[BindProperties]
public class Index : PageModel
{
    private readonly INugsService nugs;
    private readonly IPartsRepository partsRepository;

    public Index(INugsService nugs, IPartsRepository repo)
    {
        this.nugs = nugs;
        partsRepository = repo;
    }

    private static List<Part> parts = new();
    private static List<Round> rounds = new();
    private List<Zero> zeroes = new();
    public List<Part> Parts => parts;
    public List<Round> Rounds => rounds;
    public List<Zero> Zeroes => zeroes;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetAllZeroes()
    {
        string file_path = @"/home/nick/Downloads/Zeroes-Raw.csv";
        zeroes = await nugs.GetRecordsFromCSV<Zero>(file_path);
        Console.WriteLine("records found : " + zeroes.Count);
        // Thread.Sleep(5000);
        return Partial("_ZeroesGrid", this);
    }

    public async Task<IActionResult> OnGetAllParts()
    {
        string file_path = @"/home/nick/Downloads/Parts-Grid view.csv";
        parts = await nugs.GetRecordsFromCSV<Part>(file_path);
        Console.WriteLine("parts found : " + parts.Count);

        var parts_w_urls = parts.Count(p => p.Url.NotEmpty());
        var parts_wo_urls = parts.Count(p => p.Url.IsEmpty());
        Console.WriteLine("has urls: " + parts_w_urls);
        Console.WriteLine("without urls: " + parts_wo_urls);


        // var sample = parts.TakeFirstRandom();

        // sample.Dump("where url? ")
        //     ;
        // var rows = await partsRepository.Create(sample
        //     .With(m =>
        //     {
        //         m.Cost = m.Cost
        //             .Replace(@"$", "")
        //             .ToDouble(fallback: 0)
        //             .ToString();
        //     })
        // );
        // Console.WriteLine($"made {rows} new parts!");

        return Partial("_PartsGrid", this);
    }

    public async Task<IActionResult> OnGetAllRounds()
    {
        string file_path = @"/home/nick/Downloads/Rounds-Grid view.csv";
        rounds = await nugs.GetRecordsFromCSV<Round>(file_path);
        Console.WriteLine("rounds found : " + rounds.Count);
        return Partial("_RoundsGrid", this);
    }

    public async Task<IActionResult> OnGetRandomLoadout()
    {
        return Partial("_LoadoutEditor", this);
    }

    // public async Task<IActionResult> OnGetAllRecordsFromCSV(string file_path, string viewname)
    // {
    //     // todo: load specific by ui.
    //     if (viewname.IsEmpty()) return Page();
    //     return Partial(viewname, this);
    // }
}