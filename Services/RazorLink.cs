using System.Text.RegularExpressions;

namespace evantage.Models;

public record RazorLink(RazorRoute razorRoute)
{
    public string link => Regex.Replace(razorRoute.subdirectory, @"Pages\/", "") + razorRoute.file_name;
}