namespace evantage.Services;

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