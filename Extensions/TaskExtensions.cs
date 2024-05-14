using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMechanic.Extensions;

public static class TaskExtensions
{
    public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
    {
        var all_tasks = Task.WhenAll(tasks);
        try
        {
            return await all_tasks;
        }
        catch (Exception ignore_me)
        {
            // ignore the exception

            throw all_tasks.Exception ?? throw new Exception("This can't possibly throw.");
        }
    }
}