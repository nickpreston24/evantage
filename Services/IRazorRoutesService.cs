using CodeMechanic.RazorHAT.Services;

namespace evantage.Services;

public interface IRazorRoutesService2
{
    RazorRoute[] GetAllRoutes(string current_folder = "/Pages");
}