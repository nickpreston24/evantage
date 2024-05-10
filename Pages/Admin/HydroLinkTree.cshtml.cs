using Hydro;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace evantage.Pages.Admin;

[HtmlTargetElement("link-tree")]
public class HydroLinkTree : HydroView
{
    public string Folder { get; set; } // e.g. 'Sandbox'
}