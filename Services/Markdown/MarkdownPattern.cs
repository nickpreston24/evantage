using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace evantage.Services;

public class MarkdownPattern : Enumeration
{
    // https://regex101.com/r/wsbit4/1
    // Full frontmatter wip: https://regex101.com/r/S5m2wu/1
    public static MarkdownPattern FrontMatter = new MarkdownPattern(1, nameof(FrontMatter),
        pattern: @"---\n*(\s|\n|\r)((?<label>\w+):\s*(?<raw_value>.*)(\s|\n|\r))+");

    public static MarkdownPattern Link = new MarkdownPattern(2, nameof(Link), @"");

    public static MarkdownPattern Header =
        new MarkdownPattern(3, nameof(Header), @"(?<=(?<pounds>^#{1,4})\s)(?<text>.*)");

    private Regex regex;
    public readonly string pattern;

    public MarkdownPattern(int id, string name, string pattern) : base(id, name)
    {
        this.pattern = pattern;
        this.regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}