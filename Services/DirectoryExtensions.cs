using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace CodeMechanic.Migrations;

public static class DirectoryExtensions
{
    public static DirectoryInfo GoUpToDirectory(this DirectoryInfo di, string parent_name = "", bool debug = false)
    {
        // allow current directory match
        if (di.ToString().EndsWith(parent_name))
            return di;

        // throw if too high up
        string found_directory = di?.Parent?.Name ?? string.Empty;
        if (found_directory.IsEmpty())
            throw new DirectoryNotFoundException(parent_name);

        if (debug) Console.WriteLine(found_directory);

        // recurse otherwise
        return parent_name.Equals(found_directory, StringComparison.OrdinalIgnoreCase)
            ? di.Parent
            : GoUpToDirectory(di.Parent, parent_name) ?? throw new DirectoryNotFoundException(parent_name);
    }

    public static async IAsyncEnumerable<string> DiscoverDirectories(
        this DirectoryInfo di
        , Regex directory_pattern
        , bool debug = false)
    {
        if (di == null) throw new ArgumentNullException(nameof(di));
        if (directory_pattern == null) throw new ArgumentNullException(nameof(directory_pattern));

        var all_dirs = di.EnumerateDirectories("*", SearchOption.AllDirectories);

        foreach (var subdir in all_dirs)
        {
            string path = subdir.ToString();
            if (directory_pattern.IsMatch(path))
            {
                if (debug)
                    Console.WriteLine(path);
                yield return path;
            }
        }
    }
}