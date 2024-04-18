using System.Collections.Generic;

namespace CodeMechanic.Todoist;

// [JsonObject]
public class CompletedItems
{
    public List<CompletedTodoistTask> items { get; set; } = new();
}