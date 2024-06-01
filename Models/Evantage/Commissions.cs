using CodeMechanic.Diagnostics;

namespace evantage.Models;

public class Commissions
{
    public List<Line> Lines { get; set; } = new();
    public List<Device> Devices { get; set; } = new();
    public InsuranceType InsuranceType { get; set; } = InsuranceType.Allstate;

    public double Total()
    {
        double total = 0.00;
        total = Lines.Sum(line => line.InstallmentPlan.Dump("plan").GetEarnings());
        return total;
    }

    public string Post { get; set; } = "📱📱2xNU (sample)";
}