using CodeMechanic.Types;

namespace evantage.Services;

public class Direction : Enumeration
{
    public static Direction OneWay = new Direction(1, nameof(Direction.OneWay));
    public static Direction BiDirectional = new Direction(1, nameof(Direction.BiDirectional));

    public Direction(int id, string name) : base(id, name)
    {
    }
}