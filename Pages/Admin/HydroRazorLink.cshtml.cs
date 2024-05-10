using evantage.Models;
using Hydro;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace evantage.Pages.Admin;

[HtmlTargetElement("razor-link")]
public class HydroRazorLink : HydroView
{
    public RazorLink razorLink { get; set; }
}