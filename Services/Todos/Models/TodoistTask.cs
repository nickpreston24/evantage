namespace CodeMechanic.Todoist;

public class TodoistTask
{
    public string id { get; set; } = string.Empty;

    public string assigner_id { get; set; } = string.Empty;

    public string assignee_id { get; set; } = string.Empty;

    public string project_id { get; set; } = string.Empty;

    public string section_id { get; set; } = string.Empty;

    public string parent_id { get; set; } = string.Empty;

    public int order { get; set; }

    public string content { get; set; } = string.Empty;

    public string description { get; set; } = string.Empty;

    public bool is_completed { get; set; }

    public string[] labels = new string[] { };

    public string priority { get; set; } = string.Empty;

    public string comment_count { get; set; } = string.Empty;

    public string creator_id { get; set; } = string.Empty;

    public string created_at { get; set; } = string.Empty;

    public Due due { get; set; } = new();

    public string url { get; set; } = string.Empty;

    public Duration duration { get; set; } = new();
}