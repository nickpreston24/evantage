namespace evantage;

public class MyGetPackage
{
    public string Name { get; set; } = "Foo";
    public string packagetype { get; set; } = string.Empty;
    public string id { get; set; } = string.Empty;
    public string[] versions { get; set; } = Array.Empty<string>();
    public string[] dates { get; set; } = Array.Empty<string>();

    /*
    {
        "packagetype": "nuget",
        "id": "CodeMechanic.Async",
        "versions": [
        "1.0.0",
        "1.0.1",
        "1.0.2"
            ],
        "dates": [
        638188694581458630,
        638189424265114254,
        638530574083296711
            ]
    },*/
}