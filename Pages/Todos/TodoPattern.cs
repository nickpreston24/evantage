using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace CodeMechanic.Todoist;

public class TodoPattern : Enumeration
{
    // https://regex101.com/r/19ro7x/1
    public static TodoPattern ReadmeCheckbox =
        new(1, nameof(ReadmeCheckbox),
            @"(?<full_line>-\s+\[(?<is_checked_raw>(\s+|x))\]\s+(?<rest>.*))$");

    // https://regex101.com/r/Pfj9Wr/1
    public static TodoPattern Label = new TodoPattern(2, nameof(Label), @"(?<label>@(?<name>[a-zA-Z\d]+))");
    
    // https://regex101.com/r/AZroXX/1
    public static TodoPattern Priority = new TodoPattern(3, nameof(Priority), @"(?<priority>p(?<weight>\d))");
    private string _pattern { get; }
    public Regex Pattern { get; set; }

    public TodoPattern(int id, string name, string pattern) : base(id, name)
    {
        _pattern = pattern;
        Pattern = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}