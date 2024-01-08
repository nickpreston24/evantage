using CodeMechanic.Types;

namespace evantage.Models;

public class Device
{
    public string FullName { get; set; } = string.Empty; // i.e. iPhone 15
    public string Manufacturer { get; set; } = string.Empty; // Apple
    public double RetailPrice { get; set; }
    private DeviceType DeviceType { get; set; } = DeviceType.Phone;
}

public class DeviceType : Enumeration
{
    public static DeviceType Phone => new(1, nameof(Phone).ToLower());
    public static DeviceType Table => new(2, nameof(Table).ToLower());
    public static DeviceType Wallet => new(3, nameof(Wallet).ToLower());

    public DeviceType(int id, string name) : base(id, name)
    {
    }
}