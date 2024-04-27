namespace CodeMechanic.Todoist;

/// <summary>
/// A full day is a random collection of TodoistTasks that fill a slot based on priority or time.
/// </summary>
public class FullDay
{
    public List<TimeSpan> AvailableBlocksOfTime { get; set; } = new();
    public List<TodoistTask> AvailableTasks { get; set; } = new();

    public List<TodoistEvent> Schedule { get; set; } = new();

    public FullDay GetSample()
    {
        Schedule = Enumerable.Range(1, 5).Select(index => new TodoistEvent()
            {
                Task = new TodoistTask()
                {
                    description = "15min"
                },
                Duration = TimeSpan.FromMinutes(15),
            })
            .ToList();

        AvailableTasks = Schedule.Select(x => x.Task).ToList();
        AvailableBlocksOfTime = Schedule.Select(x => x.Duration).ToList();

        return this;
    }

    private TimeSpan GetAvailableBlock()
    {
        //... Find the gaps of time, then return the one closest to DateTime.Now().
        return AvailableBlocksOfTime.FirstOrDefault();
    }

    public FullDay SortByPriority()
    {
        Schedule = Schedule.OrderBy(x => x.Task.priority, StringComparer.InvariantCultureIgnoreCase).ToList();
        return this;
    }

    public FullDay SortByDuration()
    {
        Schedule = Schedule.OrderBy(x => x.Duration).ToList();
        return this;
    }
}