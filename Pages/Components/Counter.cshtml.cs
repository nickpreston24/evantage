using Hydro;
namespace evantage.Pages.Components;
// ~/Pages/Components/Counter.cshtml.cs

public class Counter : HydroComponent
{
    public int Count { get; set; }

    public void Add()
    {
        Count++;
    }
}