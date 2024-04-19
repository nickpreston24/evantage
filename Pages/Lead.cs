using System.Text;
using CodeMechanic.Types;

namespace evantage.Pages;

public class Lead
{
    public int Index { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty; // TODO: The business type code (PCI?).
    public string PhoneNumber { get; set; } = string.Empty;
    public string CallablePhoneLink => $"tel:{PhoneNumber}";
    public string State { get; set; } = "United States";
    public LeadStatus Status { get; set; } = LeadStatus.Interested;
    public CompanyRole Role { get; set; } = CompanyRole.Unknown; // e.g., Owner, Sole Prop, etc.    
    public string[] Notes { get; set; } = Array.Empty<string>();

    public override string ToString()
    {
        return new StringBuilder()
            .AppendJoin(",", CustomerName, CompanyName).ToString();
    }
}

public class CompanyRole : Enumeration
{
    public static CompanyRole Unknown = new CompanyRole(1, nameof(Unknown), description: "Role Unknown");
    public static CompanyRole Owner = new CompanyRole(2, nameof(Owner), description: "Owner");

    public static CompanyRole SoleProprietor =
        new CompanyRole(3, nameof(SoleProprietor), description: "Sole Proprietor");

    public CompanyRole(int id, string name, string description) : base(id, name)
    {
        this.Description = description;
    }

    public string Description { get; set; } = string.Empty;
}

public class LeadStatus : Enumeration
{
    public static LeadStatus Interested = new LeadStatus(1, nameof(Interested), description: "Interested");

    public static LeadStatus NotInterested = new LeadStatus(2, nameof(Interested), description: "Not Interested");
// etc...

    public LeadStatus(int id, string name, string description) : base(id, name)
    {
        this.Description = description;
    }

    public string Description { get; set; } = string.Empty;
}