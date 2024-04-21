using evantage.Models;
using Hydro;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Components;

public class LeadsPanel : HydroComponent
{
    public List<Lead> CurrentLeads { get; set; } = new();
}