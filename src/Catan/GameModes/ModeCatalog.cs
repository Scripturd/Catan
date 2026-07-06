using Catan.Game;

namespace Catan.GameModes;

public sealed class ModeCatalog
{
    public IReadOnlyList<IGameMode> Modes { get; }

    public ModeCatalog(string modesDirectory, string pluginsDirectory, Action<string>? log = null)
    {
        var modes = new List<IGameMode>();

        foreach (var definition in new BoardDefinitionLoader().LoadDirectory(modesDirectory))
            modes.Add(new DataDrivenGameMode(definition));

        modes.AddRange(new PluginLoader(log).Load(pluginsDirectory));

        Modes = modes;
    }

    public IGameMode Default => Modes[0];

    public IGameMode? ByName(string name) =>
        Modes.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
}
