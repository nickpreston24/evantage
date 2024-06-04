using System.Text;
using CodeMechanic.RegularExpressions;
using CodeMechanic.Diagnostics;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Workouts;

public class Index : PageModel
{
    private string workout_template;

    private static List<Workout> all_workouts { get; set; } = new();
    private static List<WorkoutDay> all_workout_days = new();
    private static string[] unique_exercises = Array.Empty<string>();

    public string[] UniqueExercises => unique_exercises;
    public List<Workout> AllWorkouts => all_workouts;
    public List<WorkoutDay> AllWorkoutDays => all_workout_days;

    public void OnGet()
    {
        // todo: have max_exercises be based off of lines extracted from the template
        // and also by adding reps to for future generations.
        int max_exercises = 4;

        // this template serves as a starting point for how the workout schedule will be structured
        // all new generations will fit this template (expressed in regex)
        workout_template = """  
            Friday, Air Squats: 4x5
            Friday, Treadmill: 15min
            Friday, Sit-ups: 4x5
            Friday, Push-ups: 4x5
            
            Monday, Leg Press: 5x5, 5inc
            Monday, Air Squats: 5x5, 5inc
            Monday, Treadmill: 15min
            Monday, Lats: 5x5, 3inc
            ---
            Wednesday, Leg Press: 5x5, 5inc
            Wednesday, Air Squats: 5x5, 5inc
            Wednesday, Treadmill: 15min
            Wednesday, Lats: 5x5, 3inc
            
            Wednesday, Burpees: 5x5, 5inc
            Wednesday, Air Squats: 5x5, 5inc
            Wednesday, Treadmill: 15min
            Wednesday, Lats: 5x5, 3inc
           
        """;


        var extracted_workouts = workout_template
            .Extract<Workout>(WorkoutRegex.Basic.CompiledRegex, debug: true)
            .ToList();

        var stack = new Stack<Workout>();

        foreach (var wkout in extracted_workouts)
        {
            stack.Push(wkout);
        }

        // Console.WriteLine("stack total :>> " + stack.Count);

        // extracted_workouts.FirstOrDefault().Dump(nameof(extracted_workouts));
        all_workouts = extracted_workouts;
        // Console.WriteLine($"{nameof(all_workouts)} {extracted_workouts.Count}");

        var today = DateTime.Now;

        // string dow = today.DayOfWeek.GetDescription();

        // all possible workout slots:
        all_workout_days = Enumerable.Range(0, 18)
            // setup the slots:
            .Select(index => new WorkoutDay
            {
                time = today.AddDays(index)
            })
            // .Where(wd => wd.day != "Saturday"
            //              && wd.day != "Tuesday"
            //              && wd.day != "Thursday"
            //              && wd.day != "Sunday"
            // )
            .Select(day =>
            {
                for (int i = 0; i < max_exercises; i++)
                {
                    var popped = stack.TryPop(out Workout next);
                    if (popped)
                        day.Workouts.Add(next);
                }


                return day;
            })
            // .Aggregate(new List<WorkoutDay>(), (days, next) =>
            // {
            //     return new List<WorkoutDay>()
            //     {
            //     };
            // })
            .ToList();

        // Console.WriteLine("stack remaining :>> " + stack.Count);


        // Console.WriteLine("# of workout days " + all_workout_days.Count);

        unique_exercises = all_workouts
            .DistinctBy(w => w.exercise)
            .Select(w => w.exercise)
            .ToArray();
    }

    public async Task<IActionResult> OnGetGenerateType()
    {
        Console.WriteLine(nameof(OnGetGenerateType));
        string comments = $"""
            /* 
                This is a source file automatically generated from any regex pattern  
            */
            """ ;
        /// Generate C# files:

        Thread.Sleep(5000); // TODO: remove!
        
        string sample_pattern = WorkoutRegex.Basic.Pattern;
        var pattern_bits = sample_pattern.Extract<PatternBit>(ClassGeneratingRegex.ClassMaker.CompiledRegex);
        pattern_bits.Dump(nameof(pattern_bits));

        string type_name = $"{nameof(WorkoutRegex)}DTO";
        var content = new StringBuilder(comments)
                .AppendLine("\n")
                .AppendLine($"public class {type_name} ")
                .AppendLine("{")
                .AppendEach(pattern_bits,
                    bit => $"\tpublic {GetPropertyTypeByField(bit.group_value)} {bit.group_name} " +
                           @"{ get; set; }",
                    "\n")
                .AppendLine("\n}")
                .ToString()
            ;

        string cwd = Directory.GetCurrentDirectory();
        string outfolder = "regex_to_csharp";
        string dirpath = Path.Combine(cwd, outfolder);
        var filepath = SaveAs(type_name, dirpath, content);

        return Content(filepath);
    }

    private string GetPropertyTypeByField(string field_type)
    {
        if (field_type.Contains("bit", StringComparison.OrdinalIgnoreCase))
            return "bool";
        if (field_type.Contains("int", StringComparison.OrdinalIgnoreCase))
            return "int";
        if (field_type.Equals("datetime", StringComparison.OrdinalIgnoreCase))
            return "DateTime";
        if (field_type.Contains("varchar", StringComparison.OrdinalIgnoreCase) ||
            field_type.Equals("text", StringComparison.OrdinalIgnoreCase))
            return "string";

        return "string";
    }

    private static string SaveAs(string filename, string dirpath, string content)
    {
        if (filename.IsEmpty()) throw new ArgumentNullException(nameof(filename));
        if (!Directory.Exists(dirpath))
            Directory.CreateDirectory(dirpath);
        string filepath = Path.Combine(dirpath, filename + ".generated.cs");
        System.IO.File.WriteAllText(filepath, content);
        return filepath;
    }

    public async Task<IActionResult> OnGetWorkout(string workout_id, bool debug)
    {
        // all_workouts.Dump("all owrkouts");

        if (workout_id.IsEmpty())
            return Content(nameof(OnGetWorkout) + " returned no results.");

        // if (debug)
        // Console.WriteLine(workout_id);
        // if (debug)
        //     Console.WriteLine($"{nameof(all_workouts)} {all_workouts.Count}");


        // Console.WriteLine("total slots: " + all_workout_days.DistinctBy(x => x.id).Count());
        var found_workouts = all_workout_days
            .DistinctBy(x => x.id)
            .Where(wd =>
                wd.id.NotEmpty() &&
                wd.id
                    .Equals(workout_id, StringComparison.OrdinalIgnoreCase))
            .ToList();

        // Console.WriteLine("slots found: " + found_workouts.Count);
        // if (debug)
        //     found_workouts.Dump(workout_id);

        var workouts_for_this_day = found_workouts.SelectMany(x => x.Workouts).ToList();
        return Partial("_WorkoutsList", workouts_for_this_day);
    }
}