namespace evantage.Models;

public class Line
{
    public InstallmentPlan InstallmentPlan { get; set; } = InstallmentPlan.NextUp;
    public Device Device { get; set; } = new();
}