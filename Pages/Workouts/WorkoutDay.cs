using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace evantage.Pages.Workouts;

public record WorkoutDay
{
    public string id => Regex.Replace(title, @"[\s:]", "_");
    public List<Workout> Workouts { get; set; } = new();
    public DateTime time { get; set; } = DateTime.MinValue;
    public string day => time.DayOfWeek.GetDescription();
    public string title => time.ToFriendlyDateString();

}