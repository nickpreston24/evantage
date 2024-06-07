using evantage.Models;

namespace evantage.Services;

public interface ILoadoutGenerator
{
    Task<Loadout> CreateRandomLoadout();
}