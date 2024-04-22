using System.Text.RegularExpressions;
using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.FileSystem;
using CodeMechanic.Types;

namespace evantage.Services;

public class MarkdownService : IMarkdownService
{
    public string[] AllRoutes { get; set; }

    public List<MarkdownFile> GetAllMarkdownFiles(
        string root_folder = ""
        , bool dev_mode = false)
    {
        string current_directory = root_folder.IsEmpty() ? Environment.CurrentDirectory : root_folder;
        // if (dev_mode)
        Console.WriteLine("cwd :>> " + current_directory);

        var grepper = new Grepper()
        {
            RootPath = current_directory,
            FileSearchMask = "**.md",
            Recursive = true,
            FileSearchLinePattern = MarkdownPattern.Header.pattern
        };

        var is_blacklisted = new Func<string, bool>(filepath =>
            filepath.Contains("node_modules")
            || filepath.Contains("wwwroot")
            || filepath.Contains("bin")
            || filepath.Contains("obj"));

        RegexOptions options = RegexOptions.Compiled
                               | RegexOptions.Multiline
                               | RegexOptions.IgnorePatternWhitespace
                               | RegexOptions.IgnoreCase;

        var matching_files = grepper
            .GetMatchingFiles()
            .Where(gr => !is_blacklisted(gr.FilePath))
            .ToList();

        var matching_filenames_only = grepper
                .GetFileNames()
                .Where(path => !is_blacklisted(path))
                .ToList()
            ;

        var files_containing_markdown = matching_files
                .Select(grepResult => new MarkdownFile()
                {
                    FilePath = grepResult.FilePath,
                })
                .ToList()
            ;


        var markdownFiles = matching_filenames_only
                .Select(filepath => new MarkdownFile()
                {
                    FilePath = filepath,
                })
                .ToList()
            ;

        if (dev_mode) files_containing_markdown.Dump("files containing markdown text :>> ");

        if (dev_mode) matching_filenames_only.Dump("markdown file names (only) :>> ");

        return markdownFiles;
    }
}