using Catan.Game;

namespace Catan.GameModes;

public sealed class ModeCatalog
{
    public IReadOnlyList<GameModeRegistration> Modes { get; }

    public ModeCatalog(string modesDirectory, string pluginsDirectory, Action<string>? log = null)
    {
        var modes = new List<GameModeRegistration>();

        foreach (var definition in new BoardDefinitionLoader().LoadDirectory(modesDirectory))
            modes.Add(new GameModeRegistration(definition.Name, definition.MinPlayers, definition.MaxPlayers,
                (board, tokens, harbours, robber, pirate, shuffler) =>
                    new DataDrivenGameMode(definition, board, tokens, harbours, robber, pirate, shuffler)));

        modes.AddRange(new PluginLoader(log).Load(pluginsDirectory));

        Modes = modes;
    }

    public GameModeRegistration Default => Modes[0];

    public GameModeRegistration? ByName(string name) =>
        Modes.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
}
