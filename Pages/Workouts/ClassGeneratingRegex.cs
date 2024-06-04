namespace CodeMechanic.RegularExpressions;
//
// public class ClassGeneratingRegex : RegexEnumBase
// {
//     public static ClassGeneratingRegex ClassMaker = new ClassGeneratingRegex(1, nameof(ClassMaker),
//         @"\(\?<(?<group_name>[\w\-0-9]+)>(?<group_value>.*?)\)", uri: @"https://regex101.com/r/OY7hOk/1");
//
//     protected ClassGeneratingRegex(int id, string name, string pattern, string uri) : base(id, name, pattern, uri)
//     {
//     }
// }

public class PatternBit
{
    public string group_name { get; set; } = string.Empty;
    public string group_value { get; set; } = string.Empty;
}