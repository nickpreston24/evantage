using Hydro;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace evantage.Pages.Components;

[HtmlTargetElement("shadow")]
public class Shadow : HydroView
{
    public string ShadowType { get; set; } = "basic";
}