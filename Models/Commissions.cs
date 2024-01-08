namespace evantage.Models;

public class Commissions
{
    public List<Line> Lines { get; set; } = new();
    public List<Phone> Phones { get; set; } = new();

    // public double Total { get; set; } = -5.00;
    public double Total()
    {
        double total = -5.00;

        return total;
    }

    public string Post { get; set; } = "📱📱2xNU (sample)";
}