using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeMechanic.Advanced.Regex;
using CodeMechanic.Diagnostics;
using CodeMechanic.FileSystem;
using CodeMechanic.Types;
using evantage.Models;
// using IronOcr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Leads.Import;

[BindProperties(SupportsGet = true)]
public class ImportOCR : PageModel
{
    private string images_root = "/home/nick/Downloads/google-drive/EMG/Leads";
    public List<InputPhoto> PhotoPaths { get; set; } = new();
    public int FileCount { get; set; }


    public ImportOCR()
    {
    }

    public void OnGet()
    {
    }

    private string[] GetAllImages(params string[] image_types)
    {
        string current_directory = images_root.IsEmpty() ? Environment.CurrentDirectory : images_root;
        // if (dev_mode)
        Console.WriteLine("cwd :>> " + current_directory);

        var grepper = new Grepper()
        {
            RootPath = current_directory,
            FileSearchMask = "*.jpg",
            Recursive = true,
        };

        return grepper.GetFileNames().ToArray();
    }

    public async Task<IActionResult> OnGetConvertImageToText(string file_name)
    {
        string output = await RunTesseractViaBash(file_name);
        var leads = output.Extract<Lead>(@"(?<phonenumber>\d{10,11})");
        leads.Count.Dump("leads from ocr text");

        // TODO:
        // 1. Upload all formed leads to Airtable as new leads
        // 2. Mark any leads that are malformed as 'Incomplete'.  This may be done with a formula.

        return Content(output);
    }

    private async Task<string> RunTesseractViaBash(string file_name)
    {
        Console.WriteLine(nameof(RunTesseractViaBash));
        // var output = await $"cd ~/Downloads; pwd; ls -a".Bash(); // sample
        // TODO: Finish this as a GET request:
        // string file_name = "20240422_195116.jpg";
        string out_file = file_name.RemoveFileExtension();
        Console.WriteLine("writing to: " + out_file);
        var output = await $"cd ~/Downloads/google-drive/EMG/Leads; tesseract {file_name} {out_file} ".Bash();
        Console.WriteLine(output);
        // return output;
        string result = await System.IO.File.ReadAllTextAsync(Path.Combine(images_root, out_file + ".txt"));
        return result;
    }
/*
    private void RunOCR()
    {
        throw new NotImplementedException();

        var photo_paths = new Grepper()
        {
            RootPath = images_root, Recursive = true,
            FileSearchMask = "*.jpg"
        }.GetFileNames().ToList();

        FileCount = photo_paths.Count;
        PhotoPaths = photo_paths;
        // return;
        // Attempt 2


//         var ocr = new IronTesseract();
// // Hundreds of languages available
//         ocr.Language = OcrLanguage.English;
//         using var input = new OcrInput();
//         var pageindices = new int[] { 1, 2 };
//         input.LoadImage(photo_paths.FirstOrDefault() ?? "");
// // input.DeNoise();  optional filter
// // input.Deskew();   optional filter
//         OcrResult result = ocr.Read(input);
//         Console.WriteLine(result.Text);
//         return;


        // Attempt 1

        IronTesseract ocr = new IronTesseract();
        ocr.Language = OcrLanguage.English;

        string file = photo_paths.FirstOrDefault();
        using (var Input = new OcrInput(file))
        {
            var Result = ocr.Read(Input);
            Console.WriteLine(Result.Text);
        }

        // Dictionary<string, string> results = new();
        // foreach (var file_path in photo_paths)
        // {
        //     if (!System.IO.File.Exists(file_path))
        //         continue;
        //     var result = ocr.Read(file_path);
        //     // string text = result?.Text ?? "## No values";
        //     // results.TryAdd(file_path, text); 
        // }

        // foreach (var pair in results)
        // {
        //     string file_path = pair.Key;
        //     string contents = pair.Value;
        //
        //     // System.IO.File.WriteAllText(write_path, contents);
        //     Console.WriteLine(contents);
        // }


        // var ocr_tasks = Task.Factory.StartNew(() => { });
    }
    */
}

public record InputPhoto
{
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}

public static class ShellExtensions
{
    /// <summary>
    /// Run any bash command and see the output
    /// Sources:
    ///    Basics - https://code-maze.com/csharp-execute-cli-applications/
    ///    Events - https://jackma.com/2019/04/20/execute-a-bash-script-via-c-net-core/
    ///    AVOID - CLIWrap
    /// </summary>
    /// <param name="command"></param>
    /// <param name="verbose">Show/hide output</param>
    /// <param name="writeline">Overload to whatever output function you like for verbose mode</param>
    /// <returns></returns>
    public static async Task<string> Bash(
        this string command
        , bool verbose = false
        , Action<string> writeline = null
    )
    {
        if (writeline == null)
            writeline = Console.WriteLine;

        var escapedArgs = command.Replace("\"", "\\\"");

        var psi = new ProcessStartInfo();
        // psi.FileName = "/bin/bash";
        psi.FileName = "bash";
        psi.Arguments = $"-c \"{escapedArgs}\"";
        // psi.Arguments = command;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;

        if (verbose) writeline($"Running command `{command}`");
        using var process = Process.Start(psi);

        ArgumentNullException.ThrowIfNull(process);

        // process.WaitForExit();
        await process.WaitForExitAsync();

        if (verbose) writeline("Done!");

        var output = process.StandardOutput.ReadToEnd();

        if (verbose) writeline(output);

        return output;
    }

    public static Task<int> BashJackMa(
        this string cmd
    )
    {
        var source = new TaskCompletionSource<int>();
        var escapedArgs = cmd.Replace("\"", "\\\"");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };
        process.Exited += (sender, args) =>
        {
            // logger.LogWarning(process.StandardError.ReadToEnd());
            // logger.LogInformation(process.StandardOutput.ReadToEnd());
            if (process.ExitCode == 0)
            {
                source.SetResult(0);
            }
            else
            {
                source.SetException(new Exception($"Command `{cmd}` failed with exit code `{process.ExitCode}`"));
            }

            process.Dispose();
        };

        try
        {
            process.Start();
        }
        catch (Exception e)
        {
            // logger.LogError(e, "Command {} failed", cmd);
            Console.WriteLine($"Command {cmd} failed\n{e}");
            source.SetException(e);
        }

        return source.Task;
    }
}

public class BashResult
{
    public bool Passed { get; }
    public List<Exception> Exceptions = new();
    private string command;

    public BashResult(string cmd)
    {
        command = cmd;
    }
}