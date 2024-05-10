using System.Text;

namespace evantage.Services;

public class Scripture
{
    public string Name { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string quote { get; set; } = string.Empty;
    public string quoted_text { get; set; } = string.Empty;
    public string chapter { get; set; } = string.Empty;
    public string start { get; set; } = string.Empty;
    public string end { get; set; } = string.Empty;
    public string spaces { get; set; } = string.Empty;
    public string raw_text { get; set; } = string.Empty;

    // This is here so we can easily search scripture contents
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Name)}: {Name}")
            .AppendLine($"{nameof(Text)}: {Text}")
            .AppendLine($"{nameof(quote)}: {quote}")
            .AppendLine($"{nameof(quoted_text)}: {quoted_text}")
            .AppendLine($"{nameof(chapter)}: {chapter}")
            .AppendLine($"{nameof(start)}: {start}")
            .AppendLine($"{nameof(end)}: {end}")
            .AppendLine($"{nameof(spaces)}: {spaces}")
            .AppendLine($"{nameof(raw_text)}: {raw_text}")
            .ToString();
    }
}