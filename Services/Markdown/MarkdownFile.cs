namespace evantage.Services;

public class MarkdownFile
{
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public MarkdownHeader[] Headers { get; set; } = Enumerable.Empty<MarkdownHeader>().ToArray();
    public MarkdownTable[] Tables { get; set; } = Enumerable.Empty<MarkdownTable>().ToArray();
    public MarkdownLink[] Links { get; set; } = Enumerable.Empty<MarkdownLink>().ToArray();
}