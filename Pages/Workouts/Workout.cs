using CodeMechanic.Types;

namespace evantage.Pages.Workouts;

public class Workout
{
    public int time_value { get; set; } = -1;
    public string unit_of_time { get; set; } =
        "min"; // todo: update this to support hours, and also update the regex and model

    public string day { get; set; } = string.Empty;
    public string exercise { get; set; } = string.Empty;
    public int sets { get; set; } = -1;
    public int reps { get; set; } = -1;
    public int increments { get; set; } = -1;

    public override string ToString()
    {
        // Friday, Air Squats: 4x5
        return time_value > 0 && unit_of_time.NotEmpty()
            ? $"{exercise} : for {time_value} {unit_of_time}"
            : $"{exercise} : {sets} sets of {reps}" + (increments > 0 ? $", increasing {increments} times" : "");
    }
}