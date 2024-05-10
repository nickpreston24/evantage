using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace evantage.Services;

public class ScriptureRegexPattern : Enumeration
{
    public static readonly IDictionary<ScriptureRegexPattern, Regex> compiled_patterns =
        new Dictionary<ScriptureRegexPattern, Regex>();

    public Regex CompiledPattern { get; }

    public static Regex GetPattern(ScriptureRegexPattern regexPattern)
    {
        bool is_found = compiled_patterns.TryGetValue(regexPattern, out var found);
        return is_found
            ? found
            : throw new Exception("Pattern with name '" + regexPattern.Name + "' could not be found!");
    }

    public ScriptureRegexPattern(int id
        , string name
        , string pattern
        , RegexOptions options = RegexOptions.None)
        : base(id, name)
    {
        if (options == RegexOptions.None)
            options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;

        CompiledPattern = new Regex(pattern, options);

        compiled_patterns.TryAdd(this, CompiledPattern);
    }

    public static ScriptureRegexPattern Prefixed = new ScriptureRegexPattern(1
        , nameof(Prefixed)
        , """(?<Name>[a-zA-Z]+\s+\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]+\r*\n)(?<Text>.*?)(?:\r*\n){2}""");

    public static ScriptureRegexPattern Postfixed = new ScriptureRegexPattern(2
        , nameof(Postfixed),
        """^(?<Text>.*?)(?<Name>\(\w+\s+\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{2,4}\)\.?)""");

    //https://regex101.com/r/gnRL5k/1
    public static ScriptureRegexPattern Full = new ScriptureRegexPattern(3, nameof(Full),
        """(?(?=^\s*\((?<chapter>^\d*\s*[a-zA-Z]+\s*\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{3,})\)\.)$)(“(?<quoted_text>\b[\s\.,a-zA-Z!:\d]+\b)”\s*?(?<end>\([\w\s:-]+\)\.))|((?<start>^\d*?\s*?[A-Z][\w\s:-]+$)\n(?<quote>^((\s*“?\(\d+\)[\w\s,\.!?’',”;:]+))*)(?<spaces>(”?$\n)|(”\n*?$)))""");
    
    
    // best so far:

    // (?(?=^\s*\((?<chapter>^\d*\s*[a-zA-Z]+\s*\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{3,})\)\.)$)(“(?<quoted_text>\b[\s\.,a-zA-Z!:\d]+\b)”\s*?(?<end>\([\w\s:-]+\)\.))|((?<start>^\d*?\s*?[A-Z][\w\s:-]+$)\n(?<quote>^((\s*“?\(\d+\)[\w\s,\.!?’',”;:]+(?<return>”?$\n\s*){1,}))*))

    //(?(?=^\s*\((?<chapter>^\d*\s*[a-zA-Z]+\s*\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{3,})\)\.)$)(“(?<quoted_text>\b[\s\.,a-zA-Z!:\d]+\b)”\s*?(?<end>\([\w\s:-]+\)\.))|((?<start>^\d*?\s*?[A-Z][\w\s:-]+$)\n(?<quote>^((\s*“?\(\d+\)[\w\s,\.!?’',”;:]+))*)(?<return>(”?$\n)|(”\n*?$)))

}