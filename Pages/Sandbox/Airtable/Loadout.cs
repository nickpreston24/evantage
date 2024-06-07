using evantage.Models.Csv;

namespace evantage.Models;

public class Loadout
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Inspiration { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public Part[] Medkit { get; set; } = Array.Empty<Part>().ToArray();
    public Part[] PrimaryArm { get; set; } = Array.Empty<Part>().ToArray();
    public Part[] Sidearm { get; set; } = Array.Empty<Part>().ToArray();
}