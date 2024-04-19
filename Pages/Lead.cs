namespace evantage.Pages;

public class Lead
{
    public string CustomerName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty; // TODO: The business type code (PCI?).
    public string PhoneNumber { get; set; } = string.Empty;
    public string CallablePhoneLink => $"tel:{PhoneNumber}";
}