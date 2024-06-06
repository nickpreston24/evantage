using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace evantage.Models;

public static class Globals
{
    public static bool DevMode => Environment.GetEnvironmentVariable("DEV_MODE").ToBoolean();
    public static bool DebugMode => Environment.GetEnvironmentVariable("DEBUG").ToBoolean();
    public static bool ProductionMode => Environment.GetEnvironmentVariable("PRODUCTION_MODE").ToBoolean();

    // https://regex101.com/r/JzTkRY/1
    public static bool IsLocalHost(this string url) => url.NotEmpty() &&
                                                       Regex.IsMatch(url,
                                                           @"((?<host>\d{3}\.\d{3}\.\d\.\d{3}):(?<port>\d+)\/(?<path>.*))");

    // https://regex101.com/r/JzTkRY/1
    public static bool IsRailway(this string url) => url.NotEmpty() && Regex.IsMatch(url, @"(railway\.app)");
}