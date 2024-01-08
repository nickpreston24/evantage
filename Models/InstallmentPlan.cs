using CodeMechanic.Types;

namespace evantage.Models;

public class InstallmentPlan : Enumeration
{
    public double GetEarnings()
    {
        var earning_lookup = new Dictionary<InstallmentPlan, double>()
        {
            { NextUp, 10 },
            { Other, 0 },
        };

        var _ = earning_lookup.TryGetValue(this, out var found);
        return found;
    }

    public static InstallmentPlan NextUp => new(1, nameof(NextUp).ToLower());
    public static InstallmentPlan Other => new(2, nameof(Other).ToLower());

    public InstallmentPlan(int id, string name) : base(id, name)
    {
    }
}