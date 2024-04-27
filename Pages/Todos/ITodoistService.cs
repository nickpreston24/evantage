using CodeMechanic.Curl;

namespace CodeMechanic.Todoist;

public interface ITodoistService
{
    List<CurlOptions> GetClient(string curl);

    Task<TodoistStats> GetProjectsAndTasks();
    Task<TodoistTask> UpdateTask(TodoistTask task);
    Task<List<TodoistComment>> GetTaskComments(string task_id);
}