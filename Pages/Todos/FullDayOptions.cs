namespace CodeMechanic.Todoist;

public class FullDayOptions
{
    public static FullDayOptions Default = new();
    public int take { get; set; } = 1;
    public int[] priorities { get; set; } = new[] { 1 };
    public bool take_random_todos { get; set; } = true;

    public FilterOptions Filters { get; set; } = new();
}