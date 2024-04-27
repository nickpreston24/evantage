namespace CodeMechanic.Todoist;

public class TodoistProject
{
    public string name { get; set; } = string.Empty;
    public string id { get; set; } = string.Empty;
    public string parent_id { get; set; } = string.Empty;
    public int order { get; set; } = 8;
    public string color { get; set; } = string.Empty;
    public string comment_count { get; set; } = string.Empty;
    public bool is_shared { get; set; } = false;
    public bool is_favorite { get; set; } = false;
    public bool is_inbox_project { get; set; } = false;
    public bool is_team_inbox { get; set; } = false;
    public string url { get; set; } = string.Empty;
    public string view_style { get; set; } = string.Empty;
}