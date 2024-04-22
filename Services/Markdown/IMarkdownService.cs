namespace evantage.Services;

public interface IMarkdownService
{
    string[] AllRoutes { get; set; }

    List<MarkdownFile> GetAllMarkdownFiles(string rootpath = "", bool devmode = false);
}