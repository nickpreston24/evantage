using CodeMechanic.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.CarCapital.Inventory;

[BindProperties]
public class OldAttempt : PageModel
{
    private static Car car_search { set; get; } = new(); // A static caches the previous or current search

    public Car CarSearch => car_search; // This exposes the search to our Model, for normal use.

    // Controller sample data:
    public static List<Car> cars = new()
    {
        new Car
        {
            Image_Url =
                "https://tesla-cdn.thron.com/delivery/public/image/tesla/03c34975-991c-45ee-a340-2b700bf7de01/bvlatuR/std/960x540/meet-your-tesla_model-s",
            Name = "Model S",
            Make = "Tesla"
        },
        new Car
        {
            Image_Url =
                "https://upload.wikimedia.org/wikipedia/commons/thumb/1/15/2011_Chevrolet_Volt_--_NHTSA_1.jpg/1200px-2011_Chevrolet_Volt_--_NHTSA_1.jpg",
            Name = "Chevy Volt",
            Make = "Chevy"
        }
    };

    public List<Car> AllCarInventory => cars;

    public async Task<IActionResult> OnPostFindCar([FromBody] Car car_to_find)
    {
        car_search = car_to_find.Dump("frontend search");

        var car_found = cars.FirstOrDefault();

        // return Content("ping!");
        return Partial("CarCapitalCorpCard", car_found);
    }

    public async Task<IActionResult> OnPostValidate()
    {
        car_search = cars.FirstOrDefault();
        car_search.Dump("current search");

        return Content($"""
                            <div class="card lg:card-side bg-base-100 shadow-xl">
                                <figure><img src="{car_search.Image_Url}" /></figure>
                                <div class="card-body">
                                    <h2 class="card-title">Searching for, {car_search.Name}!</h2>
                                    <p>You entered {car_search.Cost} for the cost, is this correct?</p>
                                    <div class="card-actions justify-end">
                                        <button class="btn btn-primary">Confirm</button>
                                    </div>
                                </div>
                            </div>
                        """);
    }
}