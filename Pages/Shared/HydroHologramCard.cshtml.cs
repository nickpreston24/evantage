using Hydro;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace evantage.Pages.Shared;

[HtmlTargetElement("holo-card")]
public class HydroHologramCard : HydroView
{
    public string Src { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
}