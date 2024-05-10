using System.Text.RegularExpressions;
using CodeMechanic.Advanced.Regex;
using CodeMechanic.Diagnostics;
using CodeMechanic.FileSystem;
using CodeMechanic.Migrations;
using evantage.Models;

namespace evantage.Services;

public class ScriptureService : IScriptureService
{
    private readonly IMarkdownService markdown;

    public ScriptureService(
        IMarkdownService markdownService
    )
    {
        markdown = markdownService;
    }

    public async Task<List<ScriptureFile>> GetAllScriptureFiles()
    {
        // foreach (var p in DiscoverScriptureFiles()) yield return p;
        return new List<ScriptureFile>();
    }

    private static async IAsyncEnumerable<List<Grepper.GrepResult>> DiscoverScriptureFiles()
    {
        // string[] directory_names = new[] { "tpot", "tpot-links" };
        string root_tpot_project_folder = "/tpot/tpot_static_wip";
        string json_folder = "/cache/";
        string md_folder = "/pages/";
        string cwd = Directory.GetCurrentDirectory();
        // string tpot_project_dir = Path.GetRelativePath()
        var local_dirs = cwd
            .AsDirectory()
            .GoUpToDirectory("projects");

        local_dirs.Dump(nameof(local_dirs));

        // 1. get all .md and .json files in target dir(s).
        var directory_regex = new Regex($@"(tpot)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        await foreach (var current_subdir in local_dirs.DiscoverDirectories(directory_regex))
        {
            var grepper = new Grepper
            {
                RootPath = current_subdir, Recursive = true, FileSearchMask = "*.md,*.json", FileNamePattern = "*.md"
            };

            var grep_results_batch = grepper.GetMatchingFiles().ToList();
            grep_results_batch.Count.Dump("total results");

            // 2. Return an async stream of files.
            yield return grep_results_batch.ToList();
        }
    }

    public List<Scripture> ParseLines(ScriptureRegexPattern pattern, string lines)
    {
        return lines.Extract<Scripture>();
    }

    public List<Scripture> ParseLines(
        ScriptureRegexPattern regexPattern
        , params string[] lines)
    {
        var scriptures = lines
            .SelectMany(line => line
                .Extract<Scripture>(ScriptureRegexPattern.GetPattern(regexPattern)))
            .ToList();

        return scriptures;
    }
}

public interface IScriptureService
{
    // IAsyncEnumerable<List<Grepper.GrepResult>> GetAllScriptureFiles();
    Task<List<ScriptureFile>> GetAllScriptureFiles();
}