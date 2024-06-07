namespace evantage.Pages.Dashes;

public record YoutubeVideo
{
    public string Url { get; set; } = string.Empty;
    public List<string> Notes { get; set; } = new() { "..." };
    public string Title { get; set; } = "Video";
    public YoutubeVideoStatus Status { get; set; } = YoutubeVideoStatus.Unwatched;
}