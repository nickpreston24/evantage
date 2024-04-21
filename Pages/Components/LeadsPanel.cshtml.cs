using evantage.Models;
using Hydro;

namespace evantage.Pages.Components;

public class LeadsPanel : HydroComponent
{
    public List<Lead> CurrentLeads { get; set; } = new();
}