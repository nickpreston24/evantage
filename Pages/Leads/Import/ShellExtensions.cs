using System.Diagnostics;

namespace evantage.Pages.Leads.Import;

public static class ShellExtensions
{
    /// <summary>
    /// Run any bash command and see the output
    /// Sources:
    ///    Basics - https://code-maze.com/csharp-execute-cli-applications/
    ///    Events - https://jackma.com/2019/04/20/execute-a-bash-script-via-c-net-core/
    ///    AVOID - CLIWrap
    /// </summary>
    /// <param name="command"></param>
    /// <param name="verbose">Show/hide output</param>
    /// <param name="writeline">Overload to whatever output function you like for verbose mode</param>
    /// <returns></returns>
    public static async Task<string> Bash(
        this string command
        , bool verbose = false
        , Action<string> writeline = null
    )
    {
        if (writeline == null)
            writeline = Console.WriteLine;

        var escapedArgs = command.Replace("\"", "\\\"");

        var psi = new ProcessStartInfo();
        // psi.FileName = "/bin/bash";
        psi.FileName = "bash";
        psi.Arguments = $"-c \"{escapedArgs}\"";
        // psi.Arguments = command;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;

        if (verbose) writeline($"Running command `{command}`");
        using var process = Process.Start(psi);

        ArgumentNullException.ThrowIfNull(process);

        // process.WaitForExit();
        await process.WaitForExitAsync();

        if (verbose) writeline("Done!");

        var output = process.StandardOutput.ReadToEnd();

        if (verbose) writeline(output);

        return output;
    }

    private static Task<int> BashJackMa(
        this string cmd
    )
    {
        var source = new TaskCompletionSource<int>();
        var escapedArgs = cmd.Replace("\"", "\\\"");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };
        process.Exited += (sender, args) =>
        {
            // logger.LogWarning(process.StandardError.ReadToEnd());
            // logger.LogInformation(process.StandardOutput.ReadToEnd());
            if (process.ExitCode == 0)
            {
                source.SetResult(0);
            }
            else
            {
                source.SetException(new Exception($"Command `{cmd}` failed with exit code `{process.ExitCode}`"));
            }

            process.Dispose();
        };

        try
        {
            process.Start();
        }
        catch (Exception e)
        {
            // logger.LogError(e, "Command {} failed", cmd);
            Console.WriteLine($"Command {cmd} failed\n{e}");
            source.SetException(e);
        }

        return source.Task;
    }
}