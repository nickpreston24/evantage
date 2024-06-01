using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using CodeMechanic.Types;
using Lib.AspNetCore.ServerSentEvents;

namespace evantage.Services;

public class ServerEventsWorker : BackgroundService
{
    private readonly IServerSentEventsService client;

    public ServerEventsWorker(IServerSentEventsService client)
    {
        this.client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var clients = client.GetClients();
                if (clients.Any())
                {
                    Number.Value = Enumerable.Range(100, 200).TakeFirstRandom();
                    // Console.WriteLine(Number.Value);
                    await client.SendEventAsync(
                        new ServerSentEvent
                        {
                            Id = "number",
                            Type = "number",
                            Data = new List<string>
                            {
                                Number.Value.ToString()
                            }
                        },
                        stoppingToken
                    );
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
        catch (TaskCanceledException)
        {
            // this exception is expected
        }
    }
}

public static class Number
{
    public static int Value { get; set; } = 1;
}