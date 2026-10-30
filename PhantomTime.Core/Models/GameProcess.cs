namespace PhantomTime.Core.Models;

public sealed class GameProcess
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? ExecutablePath { get; set; }

    public GameProcess(int id, string name, string? executablePath)
    {
        Id = id;
        Name = name;
        ExecutablePath = executablePath;
    }
}