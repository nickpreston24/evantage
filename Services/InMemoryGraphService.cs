using CodeMechanic.Diagnostics;
using CodeMechanic.Extensions;
using CodeMechanic.RazorHAT.Services;
using evantage.Models;
using Newtonsoft.Json;

namespace evantage.Services;

public class InMemoryGraphService : IInMemoryGraphService
{
    private InMemoryGraphOptions options;
    private readonly IJsonConfigService json_config;

    public InMemoryGraphService(IJsonConfigService json_config_service)
    {
        this.json_config = json_config_service;
        var picsum_api_results = json_config.ReadConfig("picsum.json");
        // Console.WriteLine("picsum :>> " + picsum_api_results);
        this.picsum_images = JsonConvert.DeserializeObject<List<Picsum>>(picsum_api_results);
        // picsum_images.FirstOrDefault().Dump("first pic");
    }

    public List<Picsum> picsum_images { get; set; }

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
}