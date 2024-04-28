using Hydro;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace evantage.Pages.Components;

[HtmlTargetElement("hydro-bento")]
public class HydroBento : HydroView
{
    public string columns { get; set; } = "2";
    public string rows { get; set; } = "auto";
}