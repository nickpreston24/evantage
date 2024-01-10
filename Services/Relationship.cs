namespace evantage.Services;

public class Relationship<T> where T : class
{
    public int source { get; set; }

    public int target { get; set; }
    // public Relationship(params Node<T>[] nodes)
    // {
    //     foreach (var node in nodes)
    //     {
    //         node.Relationships.TryAdd(Id, this);
    //     }
    // }

    public string Id { get; set; } = string.Empty;
    public string Label { get; set; }
    public T Properties { get; set; }
    public Direction Direction { get; set; } = Direction.OneWay;
}