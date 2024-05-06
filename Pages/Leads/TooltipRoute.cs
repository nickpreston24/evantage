namespace evantage.Models;

public record TooltipRoute
{
    public string Link { get; set; } = string.Empty;
    public string Tip { get; set; } = string.Empty;
    public object Title { get; set; } = string.Empty;
}