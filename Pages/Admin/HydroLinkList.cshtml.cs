using evantage.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Admin;

public class HydroLinkList : PageModel
{
    public RazorLink[] Links { get; set; }
    public int GridRows { get; set; } = 1; // # of grid rows desired.
    public bool GridMode => GridRows > 1;
}