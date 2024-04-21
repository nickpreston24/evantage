using CodeMechanic.Types;

namespace evantage.Models;

public class CompanyRole : Enumeration
{
    public static CompanyRole Unknown = new CompanyRole(1, nameof(Unknown), description: "Role Unknown");
    public static CompanyRole Owner = new CompanyRole(2, nameof(Owner), description: "Owner");

    public static CompanyRole SoleProprietor =
        new CompanyRole(3, nameof(SoleProprietor), description: "Sole Proprietor");

    public CompanyRole(int id, string name, string description) : base(id, name)
    {
        this.Description = description;
    }

    public string Description { get; set; } = string.Empty;
}