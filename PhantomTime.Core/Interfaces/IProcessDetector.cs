using PhantomTime.Core.Models;

namespace PhantomTime.Core.Interfaces;

public interface IProcessDetector
{
    public IReadOnlyList<GameProcess> GetRunningProcesses();
    
    public GameProcess? GetRunningProcessById(int id);
    
}