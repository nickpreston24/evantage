using CodeMechanic.Curl;

namespace CodeMechanic.Todoist;

public interface ITodoistService
{
    List<CurlOptions> GetClient(string curl);

    Task<TodoistStats> GetProjectsAndTasks();
}