namespace evantage.Models.Csv;

public class Part
{
    public int Id { get; set; } = -1;
    public string Name { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public double Cost { get; set; }
    public string Kind { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = "wwwroot/img/pewpewlogo.jpg";
}