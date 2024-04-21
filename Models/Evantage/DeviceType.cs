using CodeMechanic.Types;

namespace evantage.Models;

public class DeviceType : Enumeration
{
    public static DeviceType Phone => new(1, nameof(Phone).ToLower());
    public static DeviceType Table => new(2, nameof(Table).ToLower());
    public static DeviceType Watch => new(3, nameof(Watch).ToLower());

    public DeviceType(int id, string name) : base(id, name)
    {
    }
}