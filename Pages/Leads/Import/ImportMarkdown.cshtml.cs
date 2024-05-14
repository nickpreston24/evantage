using CodeMechanic.Advanced.Regex;
using CodeMechanic.Diagnostics;
using CodeMechanic.Markdown;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Leads.Import;

[BindProperties(SupportsGet = true)]
public class ImportFromMarkdown : PageModel
{
    private IMarkdownService markdown;
    public string MarkdownFolder = "/home/nick/Downloads/google-drive/EMG/";
    public List<MarkdownFile> LocalMarkdownFiles => _localMarkdownFiles;
    private static List<MarkdownFile> _localMarkdownFiles = new();

    public ImportFromMarkdown(IMarkdownService mkdwn)
    {
        markdown = mkdwn;
    }

    public void OnGet(string markdown_folder = "")
    {
        MarkdownFolder = markdown_folder.NotEmpty() ? markdown_folder : MarkdownFolder;
        _localMarkdownFiles = markdown.GetAllMarkdownFiles(MarkdownFolder);
        _localMarkdownFiles.Dump(nameof(_localMarkdownFiles));
    }

    public async Task<IActionResult> OnGetImport(int id = -1)
    {
        // try
        // {
            // Console.WriteLine(nameof(OnGetImport));
            // Console.WriteLine(id + " selected");
            //
            // Console.WriteLine(_localMarkdownFiles.Count);

            // var md_file = _localMarkdownFiles[id];

            var md_file = markdown.GetAllMarkdownFiles(MarkdownFolder)[id];

            md_file.Dump(nameof(md_file));


            string content = System.IO.File.ReadAllText(md_file.FilePath);
            // Console.WriteLine(content);

            var results = content.Extract<GunBrokerFFL>(GunBrokerFFLRegex.FFL.Compiled);
            // Console.WriteLine(content);
            results.FirstOrDefault().Dump("results");
            return Content($"<p class='alert alert-success'>Count: {results.Count}</p>");
        // }
        // catch (Exception e)
        // {
        //     return Content($"<p class='alert alert-error'>Failed: {e.Message}</p>");
        // }

        return Content("Import complete!");
    }
}