using CodeMechanic.Types;

namespace CodeMechanic.Todoist;

public class TodoistTask
{
    public string id { get; set; } = string.Empty;

    public string assigner_id { get; set; } = string.Empty;

    public string assignee_id { get; set; } = string.Empty;

    public string project_id { get; set; } = string.Empty;

    public string section_id { get; set; } = string.Empty;

    public string parent_id { get; set; } = string.Empty;

    public int order { get; set; }

    public string content { get; set; } = string.Empty;

    public string description { get; set; } = string.Empty;

    public bool is_completed { get; set; }

    public string[] labels = new string[] { };

    public string priority { get; set; } = string.Empty;

    public string comment_count { get; set; } = string.Empty;

    public string creator_id { get; set; } = string.Empty;

    public string created_at { get; set; } = string.Empty;

    public Due due { get; set; } = new();

    public string url { get; set; } = string.Empty;

    public Duration duration { get; set; } = new();

    /* Computed */


    public DateTime created_at_datetime => created_at.ToDateTime(fallback: DateTime.MinValue).Value;

    public string friendly_created_at => created_at_datetime.ToFriendlyDateString();

    public string priority_css
    {
        get
        {
            var value = priority.ToInt();
            switch (value)
            {
                case 4:
                    return "error";
                case 3:
                    return "warning";
                case 2:
                    return "info";
                case 1:
                default:
                    return "ghost";
            }
        }
    }

    public double Age => (due?.date?.ToDateTime(fallback: DateTime.Now) - created_at?.ToDateTime())
        .ToMaybe()
        .IfSome(x => x.TotalDays
            // src: https://www.c-sharpcorner.com/UploadFile/9b86d4/how-to-round-a-decimal-value-to-2-decimal-places-in-C-Sharp/
            // .Map(days =>
            // {
            //     days = (int)Math.Round(days, 2);
            //     return days;
            // })
        );
}