using System.Text;

namespace evantage.Pages.Todo;

public record TodoistLabel
{
    public string label { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return new StringBuilder().AppendLine($"@{Name}").ToString();
    }

    public static TodoPattern Pattern = TodoPattern.Label;
}