using System.Text.RegularExpressions;

namespace evantage.Pages.OS;

public record LocalFileSearch
{
    public string root_directory { set; get; } = string.Empty;
    public string title { set; get; } = string.Empty;
    public string tooltip { set; get; } = string.Empty;
    public string file_mask { set; get; } = string.Empty;
    public bool debug { get; set; } = false;

    public string id => Regex.Replace( title, @"\s", "_");
}