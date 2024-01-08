using CodeMechanic.Types;

namespace evantage.Models;

public class InstallmentPlan : Enumeration
{
    public static InstallmentPlan NextUp => new(1, nameof(NextUp).ToLower());
    public static InstallmentPlan Other => new(2, nameof(Other).ToLower());

    public InstallmentPlan(int id, string name) : base(id, name)
    {
    }
}