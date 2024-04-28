using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace CodeMechanic.Todoist;

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
    public static Priority Find(int id) => GetAll<Priority>().FirstOrDefault(p => p.Id == id) ?? UNKNOWN;

    public override string ToString()
    {
        return Id.ToString();
    }
}

public static class PriorityFixer
{
    private static Dictionary<int, int> fix_dict = new Dictionary<int, int>()
    {
        [4] = 1,
        [3] = 2,
        [2] = 3,
        [1] = 4
    };

    public static Priority FixPriorityBug(this string raw_todoist_priority)
    {
        var value = raw_todoist_priority.ToInt(1);
        if (value < 1 || value > 4)
            throw new ArgumentException("Invalid priority value " + value);

        fix_dict.TryGetValue(value, out int correct_value);
        return Priority.Find(correct_value);
    }
}