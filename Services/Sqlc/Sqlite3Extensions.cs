using System.Collections.Generic;
using CodeMechanic.Advanced.Regex;
using CodeMechanic.Types;

namespace CodeMechanic.Sqlc;

public static class Sqlite3Extensions
{
    public static string EscapeSingleQuotes(this string value)
    {
        var replacementMap = new Dictionary<string, string>()
        {
            { @"(?<!['\(])'{1}(?![\)',])", "''" } // https://regex101.com/r/K3O4ap/1
        };

        return value.AsArray().ReplaceAll(replacementMap).FlattenText();
    }
}