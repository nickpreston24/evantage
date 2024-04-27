using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace evantage.Pages.Todo;

public class Priority : Enumeration
{
    public static Priority UNKNOWN = new Priority(0, nameof(UNKNOWN));
    public static Priority P1 = new Priority(1, nameof(P1));
    public static Priority P2 = new Priority(2, nameof(P2));
    public static Priority P3 = new Priority(3, nameof(P3));
    public static Priority P4 = new Priority(4, nameof(P4));

    public Priority(int id, string name) : base(id, name)
    {
    }

    public static Regex Pattern = TodoPattern.Priority.Pattern;
}