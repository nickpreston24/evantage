using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using CodeMechanic.Diagnostics;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CodeMechanic.Todoist;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace evantage.Pages.Todos;

[BindProperties(SupportsGet = true)]
public class Index : PageModel
{
    public TodoistTask Todo { get; set; } = new();

    public int project_total_count { get; set; }
    public int completed_tasks_count { get; set; }
    public int all_tasks_count { get; set; }
    public int tasks_related_to_projects { get; set; }
    public string Query { get; set; } = string.Empty;

    public bool sort_by_priority { get; set; } = true;

    public int current_fullday_goal { get; set; } = 5;


    private static bool get_all_todoist_on_load = false;
    private static bool get_all_todoist_on_htmxevent = false;


    private static TodoistStats cached_todoist_stats = new();
    private static MyFullDay cached_full_day { get; set; } = new();

    private static List<MyFullDay> cached_full_week { get; set; } = new();
    public TodoistStats todoist_stats => cached_todoist_stats;

    public MyFullDay FullDay => cached_full_day;

    public List<MyFullDay> FullWeek => cached_full_week;


    private readonly ITodoistService todoist;

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

    public async Task OnGet()
    {
        Console.WriteLine(nameof(OnGet));
        cached_todoist_stats.Clear();
        // if (get_all_todoist_on_load)
        // {
        //     var stats = await this.todoist.GetProjectsAndTasks();
        //     stats.TodoistTasks = stats.TodoistTasks.ApplyFilters(new FilterOptions()
        //     {
        //         sort_by_date = new SortByDate(),
        //         sort_by_priority = new SortByPriority()
        //     });
        //
        //     cached_todoist_stats = stats;
        //
        //     project_total_count = todoist_stats.TodoistProjects.Count;
        //     completed_tasks_count = todoist_stats.CompletedTasks.Count;
        //     all_tasks_count = todoist_stats.TodoistTasks.Count;
        // }
    }


    public async Task<IActionResult> OnPostAddTodo()
    {
        string url = "https://api.todoist.com/rest/v2/tasks?content='Buy Milk zzz'";
        var todo = new TodoistTask() { content = "" };
        string json =
            // """
            //     '{"content": "Buy Milk zzz"}'
            // """;
            JsonConvert.SerializeObject(todo);
        string api_key = Environment.GetEnvironmentVariable("TODOIST_API_KEY") ?? "";
        // Console.WriteLine("Keey " + api_key);

        Console.WriteLine(json);

        try
        {
            
            // TODO: this needs fixing.  Apparently, I'm sending the wrong model? (I don't bloody know, b/c they don't send helpful message back!
            // some help: https://stackoverflow.com/questions/71013202/calling-an-api-using-httpclient-postasync-400-bad-request
            
            await RunPost(api_key, url, json, true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Content(e.Message);
        }


        return Content("added.");
    }

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
        Console.WriteLine(nameof(OnGetAllTodoistTasks));
        cached_todoist_stats.TodoistTasks = await this.todoist.SearchTodos(new TodoistTaskSearch("@coding"));
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

    private async Task<string> RunPost(string api_key, string uri, string json, bool debug = false)
    {
        using HttpClient http = new HttpClient();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", api_key);

        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Content = new StringContent(json, Encoding.UTF8);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await http.SendAsync(request);
        if (debug)
            response.Dump(nameof(response));

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return content;
    }
}

sealed class ExcludeCalculatedResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        property.ShouldSerialize = _ => ShouldSerialize(member);
        return property;
    }

    internal static bool ShouldSerialize(MemberInfo memberInfo)
    {
        var propertyInfo = memberInfo as PropertyInfo;
        if (propertyInfo == null)
        {
            return false;
        }

        if (propertyInfo.SetMethod != null)
        {
            return true;
        }

        var getMethod = propertyInfo.GetMethod;
        return Attribute.GetCustomAttribute(getMethod, typeof(CompilerGeneratedAttribute)) != null;
    }
}