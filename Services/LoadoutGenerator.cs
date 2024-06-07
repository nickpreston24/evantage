using evantage.Models;

namespace evantage.Services;

public class LoadoutGenerator : ILoadoutGenerator
{
    public async Task<Loadout> CreateRandomLoadout()
    {
        return new Loadout() { };
    }
}