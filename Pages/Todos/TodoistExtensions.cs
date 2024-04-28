using CodeMechanic.Types;
using Newtonsoft.Json;

namespace CodeMechanic.Todoist;

public static class TodoistExtensions
{
    public static List<T> Deserialize<T>(this string line) // where T : class
    {
        return JsonConvert.DeserializeObject<List<T>>(line);
    }

    public static List<TodoistTask> GetRandomFullDay(
        this List<TodoistTask> tasks
        , FullDayOptions options = default)
    {
        if (options == default)
            options = FullDayOptions.Default;

        if (options.priorities?.Length == 0)
            options.priorities = Enumerable.Range(3, 4).ToArray();

        var fullday_of_tasks = tasks
            .Where(todo => options.priorities
                // for case where there's going to be more than one, e.g. priority 2 AND 3 tasks.
                .TakeFirstRandom()
                .AsList()
                // must match the requested priorities.
                .Contains(todo.priority.FixPriorityBug().Id))

            // get random (duh)
            .If(options.take_random_todos, todos => todos
                .TakeRandom(options.take)
            )
            .ToList();

        var filtered = fullday_of_tasks.ApplyFilters(new FilterOptions()
        {
        });

        return filtered;
    }

    public static List<TodoistTask> ApplyFilters(this List<TodoistTask> todoistTasks, FilterOptions options)
    {
        return todoistTasks
            // favor older tasks
            .If(options.sort_by_date, todos => todos
                .OrderBy(todo => todo.created_at.ToDateTime())
                .ThenBy(todo => todo?.due?.date?.ToDateTime(fallback: DateTime.MinValue))
            )
            .If(options.sort_by_priority,
                todos => todos
                    .OrderBy(todo => todo.priority))
            // some Exclusions apply...
            .If(options.excepted_todo_ids.Count > 0,
                todos => todos
                    .Where(todo => !options.excepted_todo_ids
                        .Contains(todo.id)))
            .If(options.excepted_projects.Count > 0,
                todos => todos
                    .Where(todo => !options.excepted_projects
                        .Contains(todo.project_id))
            )
            // .Where(todo => !options.excepted_labels.Contains(todo.labels)) // todo: fix
            .If(options.distinct_ids_only, todos => todos
                .DistinctBy(todo => todo.id)
            )
            // Ensure unique results
            .If(options.distinct_content_only, todos => todos
                .DistinctBy(todo => todo.content)
            )
            .ToList();
    }
}