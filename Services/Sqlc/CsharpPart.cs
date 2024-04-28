using CodeMechanic.Types;

namespace CodeMechanic.Sqlc;

public class CsharpPart : Enumeration
{
    // public static Dictionary<CsharpPart, string> Parts = new()
    // {
    //     { Public, nameof(Public).ToLower() },
    //     { Static, nameof(Static).ToLower() },
    //     { Internal, nameof(Internal).ToLower() },
    //     { Sealed, nameof(Sealed).ToLower() },
    // };

    public readonly string Text;

    public CsharpPart(int id, string name, string text) : base(id, name)
    {
        Text = text;
    }

    public static CsharpPart Public = new CsharpPart(1, nameof(Public), "public");
    public static CsharpPart Static = new CsharpPart(2, nameof(Static), "static");
    public static CsharpPart Sealed = new CsharpPart(3, nameof(Sealed), "sealed");
    public static CsharpPart Internal = new CsharpPart(4, nameof(Sealed), "internal");
}