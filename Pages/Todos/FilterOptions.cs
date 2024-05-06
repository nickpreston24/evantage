using CodeMechanic.Types;

namespace CodeMechanic.Todoist;

public class FilterOptions
{
    public List<string> excluded_labels { get; set; } = new() { "bank", "coding", "computer", "guns" };
    public List<string> excluded_todo_ids { get; set; } = new();
    public List<string> excluded_projects { get; set; } = new();

    public bool distinct_ids_only { get; set; } = false;
    public bool distinct_content_only { get; set; } = false;
    public SortByDate sort_by_date { get; set; } = new();
    public SortByPriority sort_by_priority { get; set; } = new();
}

public class SortByDate : Sort
{
    public override bool Enabled { get; set; } = true;
}

public class SortByPriority : Sort
{
    public override SortDirection Direction { get; set; } = SortDirection.Descending;
    public override bool Enabled { get; set; } = true;
}

public class SortDirection : Enumeration
{
    public static SortDirection Descending = new(1, nameof(Descending));
    public static SortDirection Ascending = new(2, nameof(Ascending));

    public SortDirection(int id, string name) : base(id, name)
    {
    }
}

public abstract class Sort
{
    public Sort()
    {
        // If dev instantiates this, then always enable, because it makes sense.
        Enabled = true;
    }

    public virtual bool Enabled { set; get; } = false;
    public virtual SortDirection Direction { get; set; } = SortDirection.Ascending;
}