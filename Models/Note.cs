using CodeMechanic.Types;

namespace evantage.Models;

public class Note
{
    public string img { get; set; } = "https://picsum.photos/200/300";
    public string User { get; set; } = GetRandomUser();
    public string Description { get; set; } = "... test ...";

    private static string[] sample_users = new[]
        { "Nick", "Braden", "Nahom", "Derek", "Zuwanya", "Rosa", "Chris", "Ricci" };

    private static string GetRandomUser() => sample_users.TakeFirstRandom();
}

