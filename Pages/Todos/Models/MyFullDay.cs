namespace CodeMechanic.Todoist;

public class MyFullDay
{
    public List<TodoistTask> LowHangingFruit { get; set; } = new(); // 2x p3,p4
    public List<TodoistTask> Midday { get; set; } = new(); // 2x p2
    public List<TodoistTask> TodaysFrog { get; set; } = new(); // e.g. 1x p1

    public DateTime NextOccurence = DateTime.Now; // use for rescheduling
}