namespace evantage.Pages.Dashes;

public class Earnings
{
    public double weeks_total_gas_cost => fillups_for_the_week * average_gas_tank_cost;
    public double estimated_earnings_for_week => daily_earnings_estimate * days_run_this_week;
    public double gas_savings_for_week => weeks_total_gas_cost - (weeks_total_gas_cost * gas_discount);

    public double daily_earnings_estimate { get; set; } = 100.00;
    public double average_gas_tank_cost { get; set; } = 70.00;
    public double gas_discount = .02; // Doordash card.
    public int days_run_this_week { get; set; } = 7;
    public int fillups_for_the_week { get; set; }

    public DateTime WeekStart = DateTime.MinValue; // find the nearest past Sunday.
    public DateTime WeekEnd = DateTime.MinValue; // find the next Sunday.
    public DateTime PayDay = DateTime.MinValue; // Every Thurs.

    public string phone_app { get; set; } = string.Empty;
    // public Earnings Previous = new();
    // public Earnings Next = new();
}