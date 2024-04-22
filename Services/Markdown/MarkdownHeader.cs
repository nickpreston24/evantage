using System.Text.RegularExpressions;

namespace evantage.Services;

public class MarkdownHeader
{
    public string pounds { get; set; } = string.Empty;
    public string text { get; set; }

    // public static string header_pattern = """
    //             (?<=(?<pounds>^#{1,6})\s)(?<text>.*) # https://regex101.com/r/S8sluj/1
    //     """;
    //
    // public static Regex regexp = new Regex(header_pattern);
}