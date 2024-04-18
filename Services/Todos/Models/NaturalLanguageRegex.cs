using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace CodeMechanic.Todoist;

/// <summary>
/// Parses out a Duration using Regex instead of natural language.
/// </summary>
public class NaturalLanguageRegex : Enumeration
{
    private readonly Regex regex;

    // https://regex101.com/r/F7iIZY/1
    public static NaturalLanguageRegex Standard =
        new NaturalLanguageRegex(1, nameof(Standard),
            @"\b(?<amount>\d*)\s*(?<unit>days|minutes|min|s|seconds|sec|day)\b");

    // https://regex101.com/r/mFsCvM/1
    public static NaturalLanguageRegex MonthAndDay = new NaturalLanguageRegex(2, nameof(MonthAndDay),
        @"(?<month>(October|Dec|November))\s*(?<day>\d+(th|rd|st))");

    public NaturalLanguageRegex(int id, string name, string pattern) : base(id, name)
    {
        regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}

public static class NaturalLanguageConstants
{
    private static int[] sample_duration_amounts = new[] { 5, 10, 15, 30, 45, 60, 90, 120, 240, 360 };

    public static string[]
        days = new[]
        {
            "st", "rd", "th"
        }; // TODO: fix edge cases  like '9th', '3rd', '31st'.  It's just the 2nd digit that drives that.

    private static string[] duration_units = new[] { "day", "d", "month", "m", "year", "y" };
    private static string[] months = new[] { "November", "December", "October", "September", "August" };
    private static string[] months_abbreviated = new[] { "Nov", "Dec", "Oct", "Aug", "Sept" };
}