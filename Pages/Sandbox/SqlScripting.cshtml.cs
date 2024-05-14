#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeMechanic.Async;
using CodeMechanic.Diagnostics;
using CodeMechanic.FileSystem;
using CodeMechanic.Sqlc;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Sandbox;

[BindProperties(SupportsGet = true)]
public class SqlScripting : PageModel
{
    private readonly IGenerateSQLTypes sqlc;

    private static string cwd = string.Empty;
    private static string infolder = string.Empty;
    private static string outfolder = string.Empty;
    private static string[] targeted_files = "".AsArray();


    public string CurrentWorkingDirectory => cwd;
    public string InputFolder => infolder;
    public string OutputFolder => outfolder;
    public string[] Targeted_files => targeted_files;

    public SqlScripting(
        IGenerateSQLTypes sqlgenerator
    )
    {
        sqlc = sqlgenerator;
    }

    public async Task<IActionResult> OnGetGenerate()
    {
        var Q = new SerialQueue();
        var tasks = targeted_files
            .Select(filePath => Q
                .Enqueue(async () =>
                {
                    var options = new ScriptOptions() { };
                    var script = await sqlc.ScriptTypeAs<Models.Note>(options);
                    string filename = options.script_type.Name + filePath.RemoveFileExtension() + ".sql";
                    string outfile_path = Path.Combine(outfolder, filename);
                    Console.WriteLine($"saving as: '{outfile_path}'");
                    Console.WriteLine("text: >> " + script);
                    var fi = FS.SaveAs(new SaveAs(outfile_path), script.ToString());
                    fi.Dump("saved file");
                    
                    
                    // Thread.Sleep(1000);
                    // await ConvertSourceCode(filePath);
                    //
                    // if (SharpifierOptions.ConvertToRazorPage ||
                    //     Equals(SharpifierOptions.TargetLanguage, TargetLanguage.RazorPages))
                    //     await RefactorAsRazorPage(filePath);
                }));

        await Task.WhenAll(tasks);
        Console.WriteLine("done running");
        return Content("Done.");
    }

    public void OnGet()
    {
        cwd = Directory.GetCurrentDirectory();
        infolder = Path.GetRelativePath(cwd, @"../evantage/Models");
        outfolder = Path.GetRelativePath(cwd, @"../evantage/Sqlc");

        FS.MakeDir(outfolder);

        targeted_files = new Grepper()
            {
                RootPath = infolder
                // FileNamePattern = CSharpPattern.ClassDefinition.RawPattern
            }
            .GetFileNames()
            .Take(1)
            .Dump("cs files").ToArray();
    }
}