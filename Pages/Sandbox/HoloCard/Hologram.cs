namespace evantage.Models;

public record Hologram
{
    public string Image_Url { get; set; } =
        "https://images.unsplash.com/photo-1515266591878-f93e32bc5937?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2574&q=80";

    public string Name { get; set; } = "HOLOCARD";
}