using CodeMechanic.Types;

namespace CodeMechanic.Todoist;

public class MyFullDay
{
    public List<TodoistTask> LowHangingFruit { get; set; } = new(); // 2x p3,p4
    public List<TodoistTask> Midday { get; set; } = new(); // 2x p2
    public List<TodoistTask> TodaysFrog { get; set; } = new(); // e.g. 1x p1
    public int TotalCount => TodaysFrog.Count + Midday.Count + LowHangingFruit.Count;

    public List<string> AllIds => new List<TodoistTask>()
        .With(list =>
        {
            list.AddRange(TodaysFrog);
            list.AddRange(Midday);
            list.AddRange(LowHangingFruit);
        }).Select(todo => todo.id).ToList();

    public DateTime NextOccurence = DateTime.Now; // use for rescheduling
}