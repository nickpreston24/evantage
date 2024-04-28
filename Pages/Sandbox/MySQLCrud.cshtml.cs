using CodeMechanic.Types;
using evantage.Models;
using evantage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Sandbox;

[BindProperties(SupportsGet = true)]
public class MySQLCrud : PageModel
{
    private INotesService notes_svc;
    public List<Note> Notes => notes;
    private static List<Note> notes { get; set; } = new();

    public MySQLCrud(INotesService notes_svc)
    {
        this.notes_svc = notes_svc;
    }


    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetCreateNotes()
    {
        Console.WriteLine(nameof(OnGetCreateNotes));
        int count = 0;

        var bulk_notes = new Note() { Name = "test note zzz", Description = "this is a test note zzz" }.AsArray();

        count = await notes_svc.Create(bulk_notes);

        return Content($"Saved {count} notes");
    }
}