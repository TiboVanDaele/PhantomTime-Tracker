using System.ComponentModel;
using System.Diagnostics;
using PhantomTime.Core.Interfaces;
using PhantomTime.Core.Models;

namespace PhantomTime.Infrastructure.ProcessDetection;

public class WindowsProcessDetector : IProcessDetector
{
    // TODO: Add logging for exceptions encountered during process detection.
    // TODO: Add disposing of Processes after I'm doen using them.

    public IReadOnlyList<GameProcess> GetRunningProcesses()
    {
        var runningGameProcesses = new List<GameProcess>();
        foreach (var runningProcess in Process.GetProcesses())
        {
            string? executablePath = null;
            try
            {
                executablePath = runningProcess.MainModule.FileName;
            }
            catch (Win32Exception)
            {
                // Windows denied access to the process's executable path.
                // Keep the process anyway, without its executable path.
            }

            try
            {
                GameProcess gameProcess = new GameProcess(runningProcess.Id, runningProcess.ProcessName,
                    executablePath);
                runningGameProcesses.Add(gameProcess);
            }
            catch (Win32Exception)
            {
                // Windows denied access to information about this process.
                // Skip this process and continue with the next one.
                continue;
            }
            catch (InvalidOperationException)
            {
                // The process exited before its information could be retrieved.
                // Skip this process and continue with the next one.
                continue;
            }
            catch (NotSupportedException)
            {
                // The requested process information isn't supported.
                // Skip this process and continue with the next one.
                continue;
            }
        }

        return runningGameProcesses;
    }

    // stop time tracking when null gets returned
    public GameProcess? GetRunningProcessById(int id)
    {
        Process? runningProcess = null;
        string? executablePath = null;

        try
        {
            runningProcess = Process.GetProcessById(id);
        }
        catch (ArgumentException)
        {
            // The process ID doesn't exist or is invalid.
        }

        if (runningProcess != null)
        {
            try
            {
                executablePath = runningProcess.MainModule.FileName;
            }
            catch (Win32Exception)
            {
                // Windows denied access to the process's executable path.
            }

            try
            {
                return new GameProcess(runningProcess.Id, runningProcess.ProcessName,
                    executablePath);
            }
            catch (InvalidOperationException)
            {
                // The process exited before its information could be retrieved.
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}