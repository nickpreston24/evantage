using System.Collections.Generic;

namespace CodeMechanic.Todoist;

public class TodoistStats
{
    public List<TodoistProject> TodoistProjects { get; set; } = new();
    public List<TodoistTask> TodoistTasks { get; set; } = new();
    public List<CompletedTodoistTask> CompletedTasks { get; set; } = new();
}