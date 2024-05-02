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

    
    /** API stuff (https://developer.todoist.com/rest/v2/?shell#update-a-task)
     *
     *
     * JSON body parameters
Parameter	Required	Description
content
String
No	Task content. This value may contain markdown-formatted text and hyperlinks. Details on markdown support can be found in the Text Formatting article in the Help Center.
description
String
No	A description for the task. This value may contain markdown-formatted text and hyperlinks. Details on markdown support can be found in the Text Formatting article in the Help Center.
labels
Array of String
No	The task's labels (a list of names that may represent either personal or shared labels).
priority
Integer
No	Task priority from 1 (normal) to 4 (urgent).
due_string
String
No	Human defined task due date (ex.: "next Monday", "Tomorrow"). Value is set using local (not UTC) time. Using "no date" or "no due date" removes the date.
due_date
String
No	Specific date in YYYY-MM-DD format relative to user’s timezone.
due_datetime
String
No	Specific date and time in RFC3339 format in UTC.
due_lang
String
No	2-letter code specifying language in case due_string is not written in English.
assignee_id
String
No	The responsible user ID or null to unset (for shared tasks).
duration
Integer
No	A positive (greater than zero) integer for the amount of duration_unit the task will take, or null to unset. If specified, you must define a duration_unit.
duration_unit
String
No	The unit of time that the duration field above represents, or null to unset. Must be either minute or day. If specified, duration must be defined as well.
     */
    
    
    
    
    
    
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