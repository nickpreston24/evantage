using System.Text.RegularExpressions;
using CodeMechanic.Diagnostics;
using CodeMechanic.FileSystem;
using CodeMechanic.Migrations;

namespace evantage.Services;

public class CopyPastaService : ICopyPastaService
{
    public async IAsyncEnumerable<List<Grepper.GrepResult>> CopyAll(CopyOptions copyOptions)
    {
        if (copyOptions == null) throw new ArgumentNullException(nameof(copyOptions));

        string cwd = Directory.GetCurrentDirectory();
        var directory_regex = new Regex(copyOptions.directory_regex_pattern);

        var local_dirs = cwd
                .AsDirectory()
                .GoUpToDirectory(copyOptions.source_dir)
            ;

        await foreach (var current_subdir in local_dirs.DiscoverDirectories(directory_regex))
        {
            var grepper = new Grepper
            {
                RootPath = current_subdir, Recursive = true, FileSearchMask = copyOptions.FileSearchMask,
                FileNamePattern = copyOptions.FileNamePattern
            };

            var grep_results_batch = grepper.GetMatchingFiles();

            //  TODO: Copy / saveas each file (outside this loop, preferably)
            // foreach (var file_path in grep_results_batch.Select(res => res.FilePath))
            // {
            //     var lines = File.ReadAllLines(file_path);
            //     int lines_of_code = lines.Length;
            //     lines_of_code.Dump(nameof(lines_of_code));
            //
            // }

            yield return grep_results_batch.ToList();
        }
    }
}

public record CopyOptions
{
    public string source_dir { get; set; } = string.Empty;
    public string target_dir { get; set; } = string.Empty;
    public string directory_regex_pattern { get; set; } = ".*";
    public string FileSearchMask { get; set; } = "**.*";
    public string FileNamePattern { get; set; } = ".*";
}

public interface ICopyPastaService
{
    // Task CopyAll(CopyOptions copyOptions);
    IAsyncEnumerable<List<Grepper.GrepResult>> CopyAll(CopyOptions copyOptions);
}