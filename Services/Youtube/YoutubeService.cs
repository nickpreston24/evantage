using CodeMechanic.FileSystem;

namespace CodeMechanic.Youtube;

public class YoutubeService : IYoutubeService
{
    public async Task<List<Grepper.GrepResult>> FindAllYoutubeLinks(string directory = "~/projects/personal")
    {
        string cwd = Directory.GetCurrentDirectory();
        string rootdir = Path.GetRelativePath(cwd, directory);

        var youtube_grepper = new Grepper()
        {
            RootPath = rootdir,
            FileSearchLinePattern = @"\s*https://www.youtube.com.*"
        };

        var files = youtube_grepper.GetMatchingFiles().ToList();
        return files;
    }
}

public interface IYoutubeService
{
    Task<List<Grepper.GrepResult>> FindAllYoutubeLinks(string directory = "~/projects/personal");
}