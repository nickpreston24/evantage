using CodeMechanic.Types;

namespace CodeMechanic.Todoist;

public class Due
{
    public string date { get; set; } = string.Empty;

    // public string friendly {get;set;} = "Feb 12";
    public string lang { get; set; } = "en";
    public string is_recurring { get; set; } = "false";

    public DateTime datetime => date.ToDateTime(fallback: DateTime.MinValue).Value;
    public string friendly_date => datetime.ToFriendlyDateString();
    public string humanized_age => datetime.HumanizeAge();
    public string humanized => datetime.Humanize().ToMaybe().IfNone("Unknown");


    /**
     *
     *  friendly_name = day.ToFriendlyDateString()
            , humanized_name = day.Humanize()
     */
}