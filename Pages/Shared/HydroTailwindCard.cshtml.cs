using Hydro;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace evantage.Pages.Shared;

[HtmlTargetElement("tailwind-card")]
public class HydroTailwindCard : HydroView
{
    // Toggle this, if just showing the sample from Tailwind's website
    public bool Is_Sample { get; set; } = false;

    public string Paragraph1 { get; set; } =
        "An advanced online playground for Tailwind CSS, including support for things like:";
}