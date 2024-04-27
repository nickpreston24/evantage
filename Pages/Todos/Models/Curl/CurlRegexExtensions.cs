using System.Text.RegularExpressions;

namespace CodeMechanic.Curl;

public static class CurlRegexExtensions
{
    public static IDictionary<string, Regex> known_curls =
        new Dictionary<string, Regex>();
}