namespace CodeMechanic.Todoist;

public class TodoistComment
{
    public string id { get; set; } = string.Empty;
    public string task_id { get; set; } = string.Empty;
    public string project_id { get; set; } = string.Empty;
    public string content { get; set; } = string.Empty;
    public DateTime posted_at { get; set; }
}


/* 

  {
                "posted_at": "2020-12-20T15:47:00.956111Z",
                "attachment": {
                        "description": "The latest from the lawsuit, including a summary of yesterday's status hearing.",
                        "favicon": "https://fluoridealert.salsalabs.org/favicon.ico",
                        "resource_type": "website",
                        "title": "Federal Trial Update: New Supplement To Our TSCA Petition Submitted To Court",
                        "url": "https://fluoridealert.salsalabs.org/newsupplement?wvpId=a8f7048d-de9f-4d94-8cc7-6ed9658c1f62&fbclid=IwAR2ZYkDJGQE
u5ShNtPPPgKS5aHu7kGbeNA_O_LNDCAJ6BuSRhV9lo2QOvfI"
                }
        }
]

*/