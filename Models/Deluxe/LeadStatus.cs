using CodeMechanic.Types;

namespace evantage.Models;

public class LeadStatus : Enumeration
{
    public static LeadStatus Interested = new LeadStatus(1, nameof(Interested), description: "Interested");

    public static LeadStatus NotInterested = new LeadStatus(2, nameof(Interested), description: "Not Interested");
// etc...

    public LeadStatus(int id, string name, string description) : base(id, name)
    {
        this.Description = description;
    }

    public string Description { get; set; } = string.Empty;
}