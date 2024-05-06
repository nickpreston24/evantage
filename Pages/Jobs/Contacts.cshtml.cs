using CodeMechanic.Airtable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace evantage.Pages.Jobs;

public class Contacts : PageModel
{
    private static string job_hunt_base_id;
    private readonly IAirtableServiceV2 jobhunt_svc;
    private static List<Contact> contacts = new List<Contact>();
    private static string pat;

    public Contacts(IAirtableServiceV2 airtable)
    {
        jobhunt_svc = airtable;
    }

    public void OnGet()
    {
        job_hunt_base_id = Environment.GetEnvironmentVariable("AIRTABLE_JOB_HUNT_BASE_ID");
        pat = Environment.GetEnvironmentVariable("AIRTABLE_SALES_SPY_PAT");
        Console.WriteLine(" job baseid ::>>" + job_hunt_base_id);
    }

    public async Task<IActionResult> OnGetAllContacts()
    {
        try
        {
            Console.WriteLine(nameof(OnGetAllContacts));
            var search = new AirtableSearchV2(job_hunt_base_id, "Contacts", debug_mode: true)
            {
                airtable_pat = pat
            };

            // TODO: unencode the names, or map to correct props, somehow...
            // var contacts = await jobhunt_svc.SearchRecords<Contact>(search, debug: true);

            var fake_contacts = Enumerable.Range(1, 5).Aggregate(new List<Contact>(), (list, next) =>
            {
                list.Add(new Contact()
                {
                    Id = next, Full_Name = "Contact zzz " + next, Email = $"contact{next}@gmail.com"
                });
                return list;
            });

            return Partial("_ContactsGrid", fake_contacts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Partial("_Alert", e);
        }
        // return Content("a list of contacts");
    }
}

public class Contact
{
    public string Email { get; set; } = string.Empty;
    public string Full_Name { get; set; } = string.Empty;
    public int Id { get; set; } = -1;
}