using System.Text;

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
    public string Role { get; set; } = "Role Unknown"; // e.g., Owner, Sole Prop, etc.    

    public override string ToString()
    {
        return new StringBuilder()
            .AppendJoin(",", CustomerName, CompanyName).ToString();
    }
}