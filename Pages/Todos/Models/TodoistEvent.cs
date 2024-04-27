namespace CodeMechanic.Todoist;

public class TodoistEvent
{
    // The task at hand.
    public TodoistTask Task { get; set; } = new();

    // How long the task should take.
    public TimeSpan Duration { get; set; } = new(0);
}