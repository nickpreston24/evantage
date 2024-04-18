using System.Collections.Generic;
using System.Linq;

namespace CodeMechanic.Todoist;

public static class TodoistSerivceExtensions
{
    public static List<TodoistTask> FindTodosByProject(
        this TodoistStats todoist_stats
        , TodoistProject project)
    {
        var related_tasks = todoist_stats.TodoistProjects
            .Join(todoist_stats.TodoistTasks
                , project => project.id
                , task => task.project_id
                , (project, task) => new { project, task })
            .Where(@t => @t.project.name == project.name)
            .Select(combo => combo.task);

        return related_tasks.ToList();
    }

    public static List<CompletedTodoistTask> FindCompletedTodos(
        this TodoistStats todoist_stats
        , TodoistProject project)
    {
        var related_tasks = FindTodosByProject(todoist_stats, project);
        var all_completed_tasks = todoist_stats.CompletedTasks;

        var completed_this_project = related_tasks
            .Join(all_completed_tasks
                , project => project.id
                , completed => completed.task_id
                , (todo, completed_task) => new { todo, completed_task })
            .Select(combo => combo.completed_task)
            .ToList();

        return completed_this_project;
    }
}