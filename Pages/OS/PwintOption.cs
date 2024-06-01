namespace evantage.Pages.OS;

public record PwintOption
{
    public string title { set; get; } = string.Empty;
    public string tooltip { set; get; } = string.Empty;
    public string file_mask { set; get; } = string.Empty;
    public bool debug { get; set; } = true;

    public string id => title;
}