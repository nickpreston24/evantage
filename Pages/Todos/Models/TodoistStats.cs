namespace CodeMechanic.Todoist;

public class TodoistStats
{
    public List<TodoistProject> TodoistProjects { get; set; } = new();
    public List<TodoistTask> TodoistTasks { get; set; } = new();
    public List<CompletedTodoistTask> CompletedTasks { get; set; } = new();

    public List<TodoistComment> Comments = new();

    public int Clear()
    {
        TodoistProjects.Clear();
        TodoistTasks.Clear();
        CompletedTasks.Clear();
        Comments.Clear();

        // todo: return count of all cleared.
        return -1;
    }
}