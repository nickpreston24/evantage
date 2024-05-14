using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMechanic.Advanced.Regex;
using CodeMechanic.Diagnostics;
using CodeMechanic.FileSystem;
using CodeMechanic.RazorHAT.Services;
using CodeMechanic.Types;
using evantage.Models;
using NSpecifications;

namespace evantage.Services;

public interface IRazorRoutesService2
{
    RazorRoute[] GetAllRoutes(string current_folder, bool debug);
    RazorLink[] GetAllRazorLinks(string current_folder, bool debug = false);
}

public class RazorRoutesService2 : IRazorRoutesService2
{
    // private static RazorRoute[] razor_page_routes;

    public RazorRoutesService2(
        // string [] blacklist,
    )
    {
        // razor_page_routes = GetAllRoutes();
    }

    public RazorLink[] GetAllRazorLinks(string current_folder, bool debug = false)
    {
        var routes = GetAllRoutes(current_folder, debug = false);
        var links = routes.Select(rr => new RazorLink(rr)).ToArray();
        if (debug)
            links.Dump(nameof(links));
        return links;
    }


    public RazorRoute[] GetAllRoutes(string current_folder, bool debug = false)
    {
        if (current_folder.IsEmpty()) throw new ArgumentNullException(nameof(current_folder));

        var cwd = Directory.GetCurrentDirectory();
        if (debug) current_folder.Dump("requested folder");
        Console.WriteLine("current working dir :>> " + cwd);
        string current_page_folder_path = Path.Combine(cwd, "Pages", current_folder);
        if (debug) current_page_folder_path.Dump(nameof(current_page_folder_path));


        var grepper = new Grepper()
        {
            RootPath = current_page_folder_path,
            FileSearchMask = "*.cshtml",
            Recursive = true,
            FileSearchLinePattern = "@page"
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

        // https://regex101.com/r/qoYQzO/1
        var regex = new Regex(@"Pages/(?<subdirectory>\/?(\w+\/)*)(?<file_name>.*)\.(?<extension>cshtml)",
            options);

        // var directories = GetDirectories(current_page_folder_path);
        // if (debug) directories.Dump(nameof(directories));

        var routes = grepper.GetMatchingFiles();
        // routes.Take(2).Dump("first route");

        var razor_routes = routes.Select(rr => rr.FilePath)
                // var routes = grepper.GetFileNames()
                //         .Where(!is_blacklisted)
                //         .Where(path => !path.Contains("/Components"))
                //         .Where(path => path.StartsWith(current_page_folder_path) || path.Equals("/")
                //                                                                  || directories.Contains(path)
                //         )
                //         .Select(p => p.Replace(current_page_folder_path, ""))
                .SelectMany(p => p.Extract<RazorRoute>(regex))
                //         .DistinctBy(rr => rr.file_name)
                .ToArray()
            ;

        // razor_routes.Take(2).Dump("first extracted routes");
        return razor_routes;
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