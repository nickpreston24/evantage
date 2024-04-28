using CodeMechanic.Advanced.Regex;
using CodeMechanic.Diagnostics;
using CodeMechanic.Todoist;
using CodeMechanic.Types;
using evantage.Pages.Todo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Todos;

[BindProperties(SupportsGet = true)]
public class Index : PageModel
{
    public int project_total_count { get; set; }
    public int completed_tasks_count { get; set; }
    public int all_tasks_count { get; set; }
    public int tasks_related_to_projects { get; set; }
    public string Query { get; set; } = string.Empty;

    public bool sort_by_priority { get; set; } = true;

    public int current_fullday_goal { get; set; } = 5;

    // public TodoistStats todoist_stats { get; set; } = new();
    public TodoistStats todoist_stats => cached_todoist_stats;
    private static TodoistStats cached_todoist_stats = new();


    public MyFullDay FullDay => cached_full_day;
    private static MyFullDay cached_full_day { get; set; } = new();

    public List<MyFullDay> FullWeek => cached_full_week;
    private static List<MyFullDay> cached_full_week { get; set; } = new();

    // public List<TodoistTask> SearchResults = new();

    private readonly ITodoistService todoist;
    private readonly evantage.Services.IMarkdownService markdown;

    public Index(
        ITodoistService todos
        , evantage.Services.IMarkdownService markdown
    )
    {
        todoist = todos;
        this.markdown = markdown;
    }

    public async Task OnGet()
    {
        // throw new Exception("some dingle-headed microsoft error");
        cached_todoist_stats = await this.todoist.GetProjectsAndTasks();
        project_total_count = todoist_stats.TodoistProjects.Count;
        completed_tasks_count = todoist_stats.CompletedTasks.Count;
        all_tasks_count = todoist_stats.TodoistTasks.Count;
    }

    public async Task<IActionResult> OnGetFullDay()
    {
        if (cached_full_day.TotalCount == current_fullday_goal)
            return Partial("_FullDayCard", this);

        Console.WriteLine(nameof(OnGetFullDay));

        var todays_frog = GetRandomTasks(FullDayOptions.Default);
        var low_hanging_fruit = GetRandomTasks(new FullDayOptions()
        {
            priorities = new[] { 3, 4 }, take = 2
        });

        var midday_tasks = GetRandomTasks(new FullDayOptions()
        {
            take = 2, priorities = new[] { 2, 3 }
        });

        cached_full_day.TodaysFrog = todays_frog;
        cached_full_day.LowHangingFruit = low_hanging_fruit;
        cached_full_day.Midday = midday_tasks;

        return Partial("_FullDayCard", this);
    }

    public async Task<IActionResult> OnGetFullWeek()
    {
        Console.WriteLine(nameof(OnGetFullWeek));

        if (cached_full_week.Count == 7)
        {
            Console.WriteLine("Week already created, using cache");
            return Partial("_FullDayCard", this);
        }

        var ids_to_exclude = cached_full_day.AllIds.Dump("ids to exclude");

        var week = Enumerable.Range(1, 6).Aggregate(new List<MyFullDay>(), (list, next) =>
        {
            // Console.WriteLine("next : " + next);
            var today = new MyFullDay();

            var todays_frog = GetRandomTasks(FullDayOptions.Default
                .With(fdo => fdo.excepted_todo_ids = ids_to_exclude)
            );

            var low_hanging_fruit = GetRandomTasks(new FullDayOptions()
            {
                priorities = new[] { 3, 4 },
                take = 2,
                excepted_todo_ids = ids_to_exclude
            });

            var midday_tasks = GetRandomTasks(new FullDayOptions()
            {
                take = 2,
                priorities = new[] { 2, 3 },
                excepted_todo_ids = ids_to_exclude
            });

            today.TodaysFrog = todays_frog;
            today.LowHangingFruit = low_hanging_fruit;
            today.Midday = midday_tasks;

            list.Add(today);
            return list;
        });

        cached_full_week = week.ToList();
        cached_full_week.Count.Dump("week count");

        return Partial("_FullWeekHero", this);
    }


    private static List<TodoistTask> GetRandomTasks(FullDayOptions options = default)
    {
        if (options == default)
            options = FullDayOptions.Default;

        if (options.priorities?.Length == 0)
            options.priorities = Enumerable.Range(3, 4).ToArray();

        return cached_todoist_stats.TodoistTasks
            .Where(todo => options.priorities
                // for case where there's going to be more than one, e.g. priority 2 AND 3 tasks.
                .TakeFirstRandom()
                .AsList()
                // must match the requested priorities.
                .Contains(todo.priority.FixPriorityBug().Id))
            // favor older tasks
            .OrderBy(todo => todo.created_at.ToDateTime())
            .ThenBy(todo => todo?.due?.date?.ToDateTime(fallback: DateTime.MinValue))
            // Exclusions
            .Where(todo => !options.excepted_todo_ids.Contains(todo.id))
            // .Where(todo => !options.excepted_labels.Contains(todo.labels)) // todo: fix
            .Where(todo => !options.excepted_projects.Contains(todo.project_id))

            // Ensure unique results
            .DistinctBy(todo => todo.id)
            .DistinctBy(todo => todo.content)
            // get random (duh)
            .TakeRandom(options.take)
            .ToList();
    }

    public async Task<IActionResult> OnGetSearch()
    {
        Console.WriteLine("Query:>> " + Query);
        if (Query.IsEmpty())
            return Partial("_TodoistTasksTable", this);

        todoist_stats.TodoistTasks = todoist_stats.TodoistTasks
            .Where(x => x.ToString()
                .Contains(Query, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Partial("_TodoistTasksTable", this);
    }

    public async Task<IActionResult> OnGetFindComment(string task_id)
    {
        if (task_id.IsEmpty())
            return Content("<p class='alert'>task id is required!</p>");
        Console.WriteLine(nameof(OnGetFindComment));
        var comment = (await todoist.GetTaskComments(task_id)).FirstOrDefault();
        return Content($"<b>{comment.content}</b>");
    }

    public async Task<IActionResult> OnGetAllProjectTodos()
    {
        var readme_file = markdown
            // .Dump("all")
            .GetAllMarkdownFiles()
            .FirstOrDefault(x => x.FilePath.Contains("README", StringComparison.InvariantCultureIgnoreCase));

        string[] readme_lines = System.IO.File.ReadAllLines(readme_file.FilePath);

        var priorities = readme_lines
                .SelectMany(l => l.Extract<Priority>(Priority.Pattern))
            // .Dump("priorities")
            ;

        // string readme_text = System.IO.File.ReadAllText(readme_file.FilePath);
        // Console.WriteLine(readme_text);

        // var todos_from_readme = readme_text.Extract<ProjectTodo>(TodoPattern.ReadmeCheckbox.Pattern);
        // todos_from_readme.Dump(nameof(todos_from_readme));

        return Partial("_ProjectsTable", new List<ProjectTodo>());
    }

    public async Task<IActionResult> OnGetAllTodoistTasks()
    {
        cached_todoist_stats = await this.todoist.GetProjectsAndTasks();
        return Partial("_TodoistTasksTable", this);
    }
}

public record FullDayOptions
{
    public static FullDayOptions Default = new();
    public int take { get; set; } = 1;
    public int[] priorities { get; set; } = new[] { 1 };
    public List<string> excepted_labels = new List<string>();
    public List<string> excepted_todo_ids = new List<string>();
    public List<string> excepted_projects = new List<string>();
}