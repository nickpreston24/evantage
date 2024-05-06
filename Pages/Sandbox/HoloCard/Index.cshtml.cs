using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;

namespace evantage.Pages.CarCapital.Inventory;
//Note: to remove all comments, replace this pattern with nothing:  // .*$

[BindProperties(SupportsGet = true)]
public class Index : PageModel
{
    private readonly IEmbeddedResourceQuery embeddedResourceQuery;
    // private readonly IDriver driver;

    // Search fields
    public string Name { get; init; } = string.Empty;
    public double Cost { get; set; } = 0.0;
    public string Image_Url = string.Empty;

    // private static Car car_search { set; get; } = new(); // A static caches the previous or current search

    // public Car CarSearch => car_search; // This exposes the search to our Model, for normal use.

    public Car Search_Results { get; set; } = new Car();

    // Controller sample data:
    private static List<Car> _cars = new List<Car>();

    public List<Car> AllCarInventory => _cars;


    public Index(
        IEmbeddedResourceQuery embeddedResourceQuery
        // , IDriver driver
    )
    {
        this.embeddedResourceQuery = embeddedResourceQuery;
        // this.driver = driver;


        // _cars.Count.Dump("inventory (cached)");
        // AllCarInventory.Count.Dump("inventory");
    }

    public void OnGet()
    {
        var cars_from_db = new List<Car>()
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
            },
            new Car()
            {
                Image_Url =
                    "https://images.unsplash.com/flagged/photo-1553505192-acca7d4509be?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1yZWxhdGVkfDIwfHx8ZW58MHx8fHx8&auto=format&fit=crop&w=700&q=60",
                Name = "BMW Vista",
                Make = "BMW"
            }
        };

        _cars = cars_from_db;
    }

    // public async Task<IActionResult> OnPostValidate()
    // {
    //     return Partial("CarCapitalCorpCard",
    //         _cars
    //             .TakeFirstRandom()
    //             .Dump("requested car"));
    // }
}


/** Extra


// public async Task<IActionResult> OnPostFindCar([FromBody] Car car_to_find)
    // {
    //     Search_Results = car_to_find.Dump("frontend search");
    //
    //     var car_found = _cars.FirstOrDefault();
    //
    //     // return Content("ping!");
    //     return Partial("CarCapitalCorpCard", car_found);

        // Search_Results = cars.Dump("all cars").FirstOrDefault();
        // Search_Results.Dump("current search");

//         return Content(false
//             ? "<b>Sorry, car not found on the lot!</b>"
//             : $"""
//                    <div class="card lg:card-side bg-base-100 shadow-xl">
//                        <figure><img src="{Image_Url}" /></figure>
//                        <div class="card-body">
//                            <h2 class="card-title">Searching for, {Name}!</h2>
//                            <p>You entered {Cost} for the cost, is this correct?</p>
//                            <div class="card-actions justify-end">
//                                <button class="btn btn-primary">Confirm</button>
//                            </div>
//                        </div>
//                    </div>
//                """);
// }
**/