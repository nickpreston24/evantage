using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace evantage.Pages.Leads.Import;

public class GunBrokerFFL
{
    public string company_name { get; set; } = string.Empty;
    public string owner_name { get; set; } = string.Empty;
    public string phone_number { get; set; } = string.Empty;
    public string mobile_phone { get; set; } = string.Empty;

    public string make_default_ffl { get; set; } = string.Empty;
    public string address_1 { get; set; } = string.Empty;
    public string address_2 { get; set; } = string.Empty;
}

// Todo: persist this to a database
public class GunBrokerFFLRegex : Enumeration
{
    // https://regex101.com/r/gThyIX/1

    public static GunBrokerFFLRegex FFL = new GunBrokerFFLRegex(1, nameof(FFL),
        pattern:
        @"(#{5,})\s*(?<company_name>.*)\s*Map\sIt(?<make_default_ffl>\[.*\))(\n|\r|\s)*(\*+Address:\*+)(\n|\r|\s)*(?<owner_name>.*)(\n|\r|\s)*(?<address_1>.*)(\n|\r|\s)*(?<address_2>.*)(\n|\r|\s)*\*+Hours:\*+.*(\n|\r|\s)*\*+Phone:\**\[(?<phone_number>\d{10}).*(\n|\r|\s)*\*+Mobile:\**\[(?<mobile_phone>\d{10}).*");

    public GunBrokerFFLRegex(int id, string name, string pattern) : base(id, name)
    {
        Pattern = pattern;
        Compiled = new Regex(pattern,
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
    }

    public string Pattern { get; set; } = string.Empty;
    public Regex Compiled { get; set; }
}