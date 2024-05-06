using CodeMechanic.Types;

namespace CodeMechanic.Todoist;

public class ReadmeTodo
{
    // properties
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool Completed =>
        is_checked_raw.NotEmpty() && is_checked_raw.Contains("X", StringComparison.OrdinalIgnoreCase);


    // raw parsed values
    public string rest { get; set; } = string.Empty;
    public string tagged_words { get; set; } = string.Empty;
    public string full_line { get; set; } = string.Empty;
    public string is_checked_raw { get; set; } = string.Empty;

    public List<TodoistLabel> Labels { get; set; } = new();

    public List<TodoistProject> Projects { get; set; } = new();

    // statuses
    public Priority Priority { get; set; } = Priority.UNKNOWN;
}