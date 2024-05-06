using CodeMechanic.Advanced.Regex;
using CodeMechanic.Todoist;
using CodeMechanic.Types;

namespace evantage.Services;

public class ReadmeService : IReadmeService
{
    private readonly IMarkdownService markdown_service;

    public ReadmeService(IMarkdownService markdown)
    {
        markdown_service = markdown;
    }

    public async Task<List<ReadmeTodo>> GetAllTodosFromReadme()
    {
        var readme_file = markdown_service
            // .Dump("all")
            .GetAllMarkdownFiles()
            .FirstOrDefault(x => x.FilePath.Contains("README", StringComparison.InvariantCultureIgnoreCase));

        string[] readme_lines = System.IO.File.ReadAllLines(readme_file.FilePath);

        Console.WriteLine("README.md line count :>> " + readme_lines.Length);
        // var priorities = readme_lines
        //         .SelectMany(l => l.Extract<Priority>(Priority.Pattern))
        //     ;

        // .Dump("priorities")
        // string readme_text = System.IO.File.ReadAllText(readme_file.FilePath);
        // Console.WriteLine(readme_text);

        /*FILTERS*/
        bool sort_by_checked = true;
        var todos_from_readme = readme_lines
            .SelectMany(line => line
                .Extract<ReadmeTodo>(TodoPattern.ReadmeCheckbox.Pattern))
            .If(sort_by_checked, r => r
                .OrderByDescending(readmeTodo => readmeTodo.Completed))
            .ToList();
        return todos_from_readme;
    }
}

public interface IReadmeService
{
   Task<List<ReadmeTodo>> GetAllTodosFromReadme();
}