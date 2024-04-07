using CodeMechanic.Types;

namespace evantage.Services;

public class Node<T> where T : class
{
    public Node(T fields = null)
    {
        Fields = fields.OrNullClass();
        Label = typeof(T).Name;
    }

    public int id { get; set; }
    public string Label { get; set; }
    public T Fields { get; set; }
    public Dictionary<string, Relationship<T>> Relationships { get; set; } = new();
}