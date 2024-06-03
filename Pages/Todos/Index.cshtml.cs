using CodeMechanic.Markdown;
using CodeMechanic.Todoist;
using CodeMechanic.Types;
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


    private static bool get_all_todoist_on_load = true;
    private static bool get_all_todoist_on_htmxevent = true;


    // public TodoistStats todoist_stats { get; set; } = new();
    public TodoistStats todoist_stats => cached_todoist_stats;
    private static TodoistStats cached_todoist_stats = new();


    public MyFullDay FullDay => cached_full_day;
    private static MyFullDay cached_full_day { get; set; } = new();

    public List<MyFullDay> FullWeek => cached_full_week;
    private static List<MyFullDay> cached_full_week { get; set; } = new();

    // public List<TodoistTask> SearchResults = new();

    private readonly ITodoistService todoist;
    // private readonly IReadmeService readme_service;
    // private readonly IMarkdownService markdown;

    public Index(
        ITodoistService todos
        // , IMarkdownService markdown
        // , IReadmeService readme
    )
    {
        todoist = todos;
        // this.markdown = markdown;
        // readme_service = readme;
    }

    // public async Task OnGet()
    // {
    //     // throw new Exception("some dingle-headed microsoft error");
    //     cached_todoist_stats.Clear();
    //     if (get_all_todoist_on_load)
    //     {
    //         Console.WriteLine(nameof(OnGet));
    //         var stats = await this.todoist.GetProjectsAndTasks();
    //         stats.TodoistTasks = stats.TodoistTasks.ApplyFilters(new FilterOptions()
    //         {
    //             sort_by_date = new SortByDate(),
    //             sort_by_priority = new SortByPriority()
    //         });
    //
    //         cached_todoist_stats = stats;
    //
    //         project_total_count = todoist_stats.TodoistProjects.Count;
    //         completed_tasks_count = todoist_stats.CompletedTasks.Count;
    //         all_tasks_count = todoist_stats.TodoistTasks.Count;
    //     }
    // }

    public async Task<IActionResult> OnGetFullDay()
    {
        if (cached_full_day.TotalCount == current_fullday_goal)
            return Partial("_FullDayCard", this);

        Console.WriteLine(nameof(OnGetFullDay));

        var todays_frog = cached_todoist_stats.TodoistTasks.GetRandomFullDay(FullDayOptions.Default);
        var low_hanging_fruit = cached_todoist_stats.TodoistTasks.GetRandomFullDay(new FullDayOptions()
        {
            priorities = new[] { 3, 4 }, take = 2
        });

        var midday_tasks = cached_todoist_stats.TodoistTasks.GetRandomFullDay(new FullDayOptions()
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

        var ids_to_exclude = cached_full_day.AllIds
            // .Dump("ids to exclude")
            ;

        var week = Enumerable.Range(1, 6).Aggregate(new List<MyFullDay>(), (list, next) =>
        {
            // Console.WriteLine("next : " + next);
            var today = new MyFullDay();
            var current_tasks = cached_todoist_stats.TodoistTasks;

            var todays_frog = current_tasks.GetRandomFullDay(FullDayOptions.Default
                .With(fdo => fdo.Filters.excluded_todo_ids = ids_to_exclude)
            );

            var low_hanging_fruit = current_tasks.GetRandomFullDay(new FullDayOptions()
            {
                priorities = new[] { 3, 4 },
                take = 2,
                Filters = new FilterOptions()
                {
                    excluded_todo_ids = ids_to_exclude
                }
            });

            var midday_tasks = current_tasks.GetRandomFullDay(new FullDayOptions()
            {
                take = 2,
                priorities = new[] { 2, 3 },
                Filters = new FilterOptions()
                {
                    excluded_todo_ids = ids_to_exclude
                }
                // FilteringOptions.excluded_todo_ids = ids_to_exclude
            });

            today.TodaysFrog = todays_frog;
            today.LowHangingFruit = low_hanging_fruit;
            today.Midday = midday_tasks;

            list.Add(today);
            return list;
        });

        cached_full_week = week.ToList();
        // cached_full_week.Count.Dump("week count");

        return Partial("_FullWeekHero", this);
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

    // TODO :Uncomment and fix... I refactored the readme_service to be in CodeMechanic, which was a mistake.
    // public async Task<IActionResult> OnGetAllReadmeTodos()
    // {
    //     var todos_from_readme = await readme_service.GetAllTodosFromReadme();
    //
    //     Console.WriteLine("total todos found in README.md: " + todos_from_readme.Count);
    //
    //     return Partial("_ReadmeTable", todos_from_readme);
    // }


    public async Task<IActionResult> OnGetAllTodoistTasks()
    {
        if (get_all_todoist_on_htmxevent)
        {
            var stats = await this.todoist.GetProjectsAndTasks();

            stats.TodoistTasks = stats.TodoistTasks.ApplyFilters(new FilterOptions()
            {
            });

            cached_todoist_stats = stats;
        }

        return Partial("_TodoistTasksTable", this);
    }

    public async Task<IActionResult> OnGetToggleCompleted(string task_id, bool completed = false)
    {
        Console.WriteLine(nameof(OnGetToggleCompleted));

        if (task_id.IsEmpty())
            return Content("Could not update an empty task id.");

        try
        {
            // string json = 

            // string json = "{}"; // (new TodoistTask() { id = task_id, is_completed = completed }).Serialize();

            var todo = cached_todoist_stats.TodoistTasks
                    .SingleOrDefault(t => t.id == task_id)
                    .With(t =>
                    {
                        t.id = task_id;
                        t.is_completed = completed;
                    })
                ;

            var result = await todoist.UpdateTask(todo);
            return Content($"<p class='alert alert-success'>Task completed {task_id} set to {completed}<p>");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Content($"<p class='alert alert-error'>{e.Message}</p>");
        }
    }


    public async Task<IActionResult> OnGetSetPriority(string task_id, string priority)
    {
        // Todo: needs fixing
        Console.WriteLine(nameof(OnGetSetPriority));
        // task_id.Dump(nameof(task_id));
        // priority.Dump(nameof(priority));
        if (task_id.IsEmpty())
            return Content("Could not update an empty task id.");
        if (priority.IsEmpty())
            return Content("Could not update an empty priority.");

        // try
        // {
        //     var result = await todoist.UpdateTask(new TodoistTask() { id = task_id, priority = priority });
        //     return Content("Priority set to " + result.priority);
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        //     return Content($"<p class='alert alert-error'>{e.Message}</p>");
        // }
        return Content($"<p class='alert alert-success'>Task {task_id} set to Priority {priority}<p>");
    }
}