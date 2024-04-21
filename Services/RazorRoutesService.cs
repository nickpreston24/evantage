using System.Text.RegularExpressions;
using CodeMechanic.Advanced.Regex;
using CodeMechanic.Diagnostics;
using CodeMechanic.FileSystem;
using CodeMechanic.RazorHAT.Services;
using NSpecifications;

namespace evantage.Services;

public class RazorRoutesService2 : IRazorRoutesService2
{
    private readonly bool dev_mode;
    // private static RazorRoute[] razor_page_routes;

    public RazorRoutesService2(
        // string [] blacklist,
        bool dev_mode = false)
    {
        // razor_page_routes = GetAllRoutes();
        this.dev_mode = dev_mode;
    }

    public RazorRoute[] GetAllRoutes(string current_folder = "/")
    {
        string current_directory = Directory.GetCurrentDirectory() + "/Pages" + current_folder;
        if (dev_mode)
            Console.WriteLine("CWD2 :>> " + current_directory);


        var grepper = new Grepper()
        {
            RootPath = current_directory,
            FileSearchMask = "*.cshtml",
            Recursive = true,
        };

        var is_blacklisted = new Spec<string>(
            filepath =>
                filepath.Contains("node_modules/")
                || filepath.Contains("wwwroot/")
                || filepath.Contains("bin/")
                || filepath.Contains("obj/")
        );

        RegexOptions options = RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace |
                               RegexOptions.IgnoreCase;

        var regex = new Regex(@"(?<subdirectory>\/?(\w+\/)*)(?<file_name>.*\.(?<extension>cshtml|cs))",
            options);

        Console.WriteLine(current_directory);
        var directories = GetDirectories(current_directory);
        directories.Dump("dirs");

        var routes = grepper.GetFileNames()
                .Where(!is_blacklisted)
                .Where(path => !path.Contains("/Components"))
                .Where(path => path.StartsWith(current_directory) || path.Equals("/")
                                                                  || directories.Contains(path)
                )
                .Select(p => p.Replace(current_directory, ""))
                .SelectMany(p => p.Extract<RazorRoute>(regex))
                .DistinctBy(rr => rr.file_name)
            ;

        return routes.ToArray();
    }

    public string[] GetBreadcrumbsForPage(string page_name)
    {
        throw new NotImplementedException(
            "Not used here yet, and I want to change it to something more useful anyways.");
        // var current_breadcrumbs = this.GetAllRoutes()
        //         .SelectMany(x => x.subdirectory)
        //         .Where(path => path.Contains(page_name))
        //     // .Dump("Current breadcrumbs")
        //     ;
        //
        // return current_breadcrumbs.ToArray();
    }


    public static List<string> GetDirectories(string path
        , string searchPattern = "*"
        , SearchOption searchOption = SearchOption.AllDirectories)
    {
        if (searchOption == SearchOption.TopDirectoryOnly)
            return Directory.GetDirectories(path, searchPattern).ToList();

        var directories = new List<string>(GetDirectories(path, searchPattern));

        for (var i = 0; i < directories.Count; i++)
            directories.AddRange(GetDirectories(directories[i], searchPattern));

        return directories;
    }

    private static List<string> GetDirectories(string path, string searchPattern)
    {
        try
        {
            return Directory.GetDirectories(path, searchPattern).ToList();
        }
        catch (UnauthorizedAccessException)
        {
            return new List<string>();
        }
    }
}