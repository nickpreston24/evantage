namespace CodeMechanic.Todoist;

public class CompletedTodoistTask
{
    public string completed_at { get; set; } = string.Empty;
    public string item_object { get; set; } = string.Empty;
    public string meta_data { get; set; } = string.Empty;

    public int note_count { get; set; } = 0;

    public string[] notes { get; set; } = new string[] { };
    public string task_id { get; set; } = string.Empty;
    public string user_id { get; set; } = string.Empty;
}