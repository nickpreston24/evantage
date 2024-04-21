using System.Text;

namespace evantage.Models;

public class Lead
{
    public int Index { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty; // TODO: The business type code (PCI?).
    public string PhoneNumber { get; set; } = string.Empty;
    public string CallablePhoneLink => $"tel:{PhoneNumber}";
    public string State { get; set; } = "United States";
    public string[] Notes { get; set; } = Array.Empty<string>();
    public LeadStatus Status { get; set; } = LeadStatus.Interested;
    public CompanyRole Role { get; set; } = CompanyRole.Unknown; // e.g., Owner, Sole Prop, etc.    
    public DataSource DataSource = DataSource.Airtable; // What kind of persistence layer does this lead come from?

    public override string ToString()
    {
        return new StringBuilder()
            .AppendJoin(",", CustomerName, CompanyName).ToString();
    }
}