namespace CodeMechanic.Todoist;

public class FilterOptions
{
    public List<string> excepted_labels { get; set; } = new List<string>();
    public List<string> excepted_todo_ids { get; set; } = new List<string>();
    public List<string> excepted_projects { get; set; } = new List<string>();

    public bool distinct_ids_only { get; set; } = false;
    public bool distinct_content_only { get; set; } = false;
    public bool sort_by_date { get; set; } = true;
    public bool sort_by_priority { get; set; } = false;
}