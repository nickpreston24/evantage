namespace evantage.Models;

public class CounterBoxOptions
{
    public string Label { get; set; } = string.Empty;
    public int Count { get; set; }
    public static CounterBoxOptions None { get; set; } = new();
}