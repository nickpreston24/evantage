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
    public string Status { get; set; } = LeadStatus.Interested.Description;
    public string Role { get; set; } = "Role Unknown"; // e.g., Owner, Sole Prop, etc.    
    public string[] Notes { get; set; } = Array.Empty<string>();

    public override string ToString()
    {
        return new StringBuilder()
            .AppendJoin(",", CustomerName, CompanyName).ToString();
    }
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