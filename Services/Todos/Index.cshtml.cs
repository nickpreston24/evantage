using CodeMechanic.Todoist;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrashStack.Pages.Todos;

public class Index : PageModel
{
    public int project_total_count { get; set; }
    public int completed_tasks_count { get; set; }
    public int all_tasks_count { get; set; }
    public int tasks_related_to_projects { get; set; }

    public TodoistStats todoist_stats { get; set; } = new();

    private readonly ITodoistService todoist;


    public Index(ITodoistService todos)
    {
        todoist = todos;
    }

    public async Task OnGet()
    {
        todoist_stats = await this.todoist.GetProjectsAndTasks();
        project_total_count = todoist_stats.TodoistProjects.Count;
        completed_tasks_count = todoist_stats.CompletedTasks.Count;
        all_tasks_count = todoist_stats.TodoistTasks.Count;
    }
}