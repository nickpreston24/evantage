using CodeMechanic.RegularExpressions;

namespace evantage.Pages.Workouts;

public class WorkoutRegex : RegexEnumBase
{
    public static WorkoutRegex Basic = new WorkoutRegex(1, nameof(Basic),
        pattern:
        // https://regex101.com/r/a5DxKM/1
        @"(?<day>\w+),\s*(?<exercise>[\w\s-]+):\s*((?<sets>\d+)x(?<reps>\d+)|(?<time_value>\d+)(?<unit_of_time>min))(,\s*(?<increments>\d+)inc)?");

    protected WorkoutRegex(int id, string name, string pattern) : base(id, name, pattern)
    {
    }
}