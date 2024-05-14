using System.Collections.Generic;
using System.Threading.Tasks;
using CodeMechanic.Diagnostics;
using CodeMechanic.Scriptures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Scriptures;

[BindProperties(SupportsGet = true)]
public class Index : PageModel
{
    private readonly IScriptureService scriptures_svc;
    private static List<Scripture> all_scriptures = new();
    public List<Scripture> Scriptures => all_scriptures;

    private static List<ScriptureFile> all_scripture_files = new();
    public List<ScriptureFile> ScriptureFiles => all_scripture_files;


    public string Query { get; set; } = string.Empty;

    public Index(IScriptureService scriptureService)
    {
        scriptures_svc = scriptureService;
    }

    public async Task<IActionResult> OnGetSearchScriptures(string query)
    {
        query.Dump(nameof(query));
        return Partial("_ScriptureFilesList", all_scripture_files);
    }

    public async Task<IActionResult> OnGetAllLocalScriptures()
    {
        var collection = await scriptures_svc.GetAllScriptureFiles();


        return Partial("_ScriptureFilesList", all_scripture_files);
    }

    public void OnGet()
    {
    }
}