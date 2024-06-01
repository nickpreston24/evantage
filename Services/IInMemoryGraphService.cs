using evantage.Models;

namespace evantage.Services;

public interface IInMemoryGraphService
{
    InMemoryGraphService SetOptions(InMemoryGraphOptions options);
    InMemoryGraphService LoadOptions(string filename, bool debug_mode = false);
    List<Picsum> picsum_images { get; set; }
}