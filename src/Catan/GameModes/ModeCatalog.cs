using Catan.Game;

namespace Catan.GameModes;

public sealed class ModeCatalog
{
    public IReadOnlyList<IGameMode> Modes { get; }

    public ModeCatalog(string modesDirectory, IEnumerable<IGameMode> builtIns)
    {
        var modes = new List<IGameMode>();

        foreach (var definition in new BoardDefinitionLoader().LoadDirectory(modesDirectory))
            modes.Add(new DataDrivenGameMode(definition));

        modes.AddRange(builtIns);

        Modes = modes;
    }

    public IGameMode Default => Modes[0];

    public IGameMode? ByName(string name) =>
        Modes.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
}