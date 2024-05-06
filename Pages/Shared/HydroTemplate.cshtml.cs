using Hydro;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace evantage.Pages.Shared;

[HtmlTargetElement("hydro-template")]
public class HydroTemplate : HydroView
{
    public bool Show { get; set; } = false;
}
