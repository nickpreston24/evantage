using CodeMechanic.Types;

namespace evantage.Models;

public class InsuranceType : Enumeration, ICommissionType
{
    public static InsuranceType Allstate = new InsuranceType(1, nameof(Allstate));
    public static InsuranceType Individual = new InsuranceType(2, nameof(Individual));
    public static InsuranceType None = new InsuranceType(3, nameof(None));

    public InsuranceType(int id, string name) : base(id, name)
    {
    }

    public string Abbreviation { get; } = "Ins";
}