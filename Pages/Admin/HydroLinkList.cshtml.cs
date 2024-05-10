using evantage.Models;
using Hydro;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace evantage.Pages.Admin;

[HtmlTargetElement("link-list")]
public class HydroLinkList : HydroView
{
    public string Folder { get; set; } = "Sandbox"; // e.g. 'Sandbox'
    public RazorLink[] Links { get; set; }
    public int GridRows { get; set; } = 1; // # of grid rows desired.
    public bool GridMode => GridRows > 1;
}