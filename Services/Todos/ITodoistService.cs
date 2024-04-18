using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMechanic.Todoist;

public interface ITodoistService
{
    List<CurlOptions> GetClient(string curl);

    Task<TodoistStats> GetProjectsAndTasks();
}