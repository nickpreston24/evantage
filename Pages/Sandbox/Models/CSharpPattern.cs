#nullable enable
using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace evantage.Pages.Sandbox;

public class CSharpPattern : Enumeration
{
    // https://regex101.com/r/Q1cMsY/1
    public static CSharpPattern ClassDefinition = new CSharpPattern(1, nameof(ClassDefinition),
        pattern:
        @"(public|static|internal|private)\s*(class|record|struct)\s[\w\d_]+\s+(?!:\s(PageModel|HydroComponent:HydroView))");

    public Regex Pattern { get; set; }
    public string RawPattern { get; set; } = string.Empty;

    public CSharpPattern(int id, string name, string pattern) : base(id, name)
    {
        Pattern = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        RawPattern = pattern;
    }
}