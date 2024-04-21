namespace evantage.Models;

public class Device
{
    public string FullName { get; set; } = string.Empty; // i.e. iPhone 15
    public string Manufacturer { get; set; } = string.Empty; // Apple
    public double RetailPrice { get; set; }
    private DeviceType DeviceType { get; set; } = DeviceType.Phone;
}