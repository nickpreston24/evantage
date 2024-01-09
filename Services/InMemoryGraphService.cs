using CodeMechanic.RazorHAT.Services;
using CodeMechanic.Types;
using Newtonsoft.Json;

namespace evantage.Services;

public class InMemoryGraphService : IInMemoryGraphService
{
    private InMemoryGraphOptions options;
    private readonly IJsonConfigService json_config;

    public InMemoryGraphService(IJsonConfigService json_config_service)
    {
        this.json_config = json_config_service;
    }

    public InMemoryGraphService SetOptions(InMemoryGraphOptions options)
    {
        this.options = options;
        return this;
    }

    public InMemoryGraphService LoadOptions(string filename, bool debug_mode = false)
    {
        var raw_settings_text = json_config.ReadConfig(filename);
        if (debug_mode) Console.WriteLine("raw settings :>> " + raw_settings_text);
        var graph_options = JsonConvert.DeserializeObject<InMemoryGraphOptions>(raw_settings_text);
        options = graph_options;
        return this;
    }

    public List<Node<T>> LoadGraph<T>() where T : class
    {
        return new List<Node<T>>();
    }


    public List<Node<T>> GetNodes<T>() where T : class
    {
        return new List<Node<T>>();
    }

    // async adapted from: https://www.youtube.com/watch?v=lQu-eBIIh-w

    // public static async Task<InMemoryGraphService> CreateAsync()
    // {
    //     return new InMemoryGraphService(await Task.Run(() =>
    //     {
    //         ...
    //         
    //     }));
    // }
}

public class Node<T> where T : class
{
    public string Id { get; set; }
    public string Label { get; set; }
    public T Fields { get; set; }
    public Dictionary<string, Relationship<T>> Relationships { get; set; } = new();
}

public class Relationship<T> where T : class
{
    public Relationship(params Node<T>[] nodes)
    {
        foreach (var node in nodes)
        {
            node.Relationships.TryAdd(Id, this);
        }
    }

    public string Id { get; set; } = string.Empty;
    public string Label { get; set; }
    public T Properties { get; set; }
    public Direction Direction { get; set; } = Direction.OneWay;
}

public class Direction : Enumeration
{
    public static Direction OneWay = new Direction(1, nameof(Direction.OneWay));
    public static Direction BiDirectional = new Direction(1, nameof(Direction.BiDirectional));

    public Direction(int id, string name) : base(id, name)
    {
    }
}

public class InMemoryGraphOptions
{
    public int NodeLimit { get; set; } = 100;

    public async Task<InMemoryGraphOptions> LoadOptionsAsync()
    {
        // TODO: Deserialize JSON
        var task = Task.Run(() => new InMemoryGraphOptions());
        return await task;
    }
}

public interface IInMemoryGraphService
{
    InMemoryGraphService SetOptions(InMemoryGraphOptions options);
    InMemoryGraphService LoadOptions(string filename, bool debug_mode = false);
    List<Node<T>> LoadGraph<T>() where T : class;
    List<Node<T>> GetNodes<T>() where T : class;
}