namespace CodeMechanic.Todoist;

public class ProjectTodo
{
    // properties
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    private bool Completed { get; set; }


    // raw parsed values
    public string rest { get; set; } = string.Empty;
    public string tagged_words { get; set; } = string.Empty;
    public string full_line { get; set; } = string.Empty;
    public bool is_checked_raw { get; set; }

    public List<TodoistLabel> Labels { get; set; } = new();

    public List<TodoistProject> Projects { get; set; } = new();

    // statuses
    public Priority Priority { get; set; } = Priority.UNKNOWN;
}