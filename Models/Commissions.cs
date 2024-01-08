namespace evantage.Models;

public class Commissions
{
    public List<Line> Lines { get; set; } = new();
    public List<Phone> Phones { get; set; } = new();

    public double Total { get; set; } = -1.00;
    public string Post { get; set; } = "📱📱2xNU";
}