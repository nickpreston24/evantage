using CodeMechanic.Types;

namespace evantage.Models;

public class PhonePlan : Enumeration
{
    public static PhonePlan Starter = new PhonePlan(1, nameof(Starter));
    public static PhonePlan Extra = new PhonePlan(2, nameof(Extra));
    public static PhonePlan Premium = new PhonePlan(3, nameof(Premium));

    public PhonePlan(int id, string name) : base(id, name)
    {
    }
}