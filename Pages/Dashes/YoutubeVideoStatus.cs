using CodeMechanic.Types;

namespace evantage.Pages.Dashes;

public class YoutubeVideoStatus : Enumeration
{
    public static YoutubeVideoStatus Unwatched = new YoutubeVideoStatus(1, nameof(Unwatched));
    public static YoutubeVideoStatus Rewatch = new YoutubeVideoStatus(2, nameof(Rewatch));
    public static YoutubeVideoStatus WatchLater = new YoutubeVideoStatus(3, nameof(WatchLater));
    public static YoutubeVideoStatus Watched = new YoutubeVideoStatus(4, nameof(Watched));

    public YoutubeVideoStatus(int id, string name) : base(id, name)
    {
    }

    public static implicit operator YoutubeVideoStatus(string name)
    {
        var enumeration = GetAll<YoutubeVideoStatus>()
            .SingleOrDefault(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        return enumeration;
    }
}