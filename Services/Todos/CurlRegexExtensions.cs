using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeMechanic.Todoist;

public static class CurlRegexExtensions
{
    public static IDictionary<string, Regex> known_curls =
        new Dictionary<string, Regex>();
}