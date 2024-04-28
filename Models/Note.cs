using CodeMechanic.Types;

namespace evantage.Models;

public class Note
{
    public string Id { get; set; } = string.Empty;
    public string img { get; set; } = "https://picsum.photos/200/300";
    public string Name { get; set; } = string.Empty;
    public string User { get; set; } = GetRandomUser();
    public string Description { get; set; } = "... test ...";
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }

    private static string[] sample_users = new[]
        { "Nick", "Braden", "Nahom", "Derek", "Zuwanya", "Rosa", "Chris", "Ricci" };

    private static string GetRandomUser() => sample_users.TakeFirstRandom();
}