using Catan.Game;
using Catan.Modding;
using Catan.SeafarersScenario1;
using Catan.Standard;

namespace Catan.Server;

public sealed class ModeCatalog
{
    public IReadOnlyList<GameModeRegistration> Modes { get; }

    public ModeCatalog(string modesDirectory, IEnumerable<GameModeRegistration>? pluginModes = null)
    {
        var modes = new List<GameModeRegistration>
        {
            new("Standard Catan", 3, 4,
                (board, tokens, harbours, robber, _, shuffler) =>
                    new StandardGame(board, tokens, harbours, robber, shuffler)),
            new("Seafarers: Heading for New Shores", 3, 4,
                (board, tokens, harbours, robber, pirate, shuffler) =>
                    new SeafarersScenario1Game(board, tokens, harbours, robber, pirate, shuffler)),
        };

        foreach (var definition in new BoardDefinitionLoader().LoadDirectory(modesDirectory))
            modes.Add(new GameModeRegistration(definition.Name, definition.MinPlayers, definition.MaxPlayers,
                (board, tokens, harbours, robber, pirate, shuffler) =>
                    new DataDrivenGameMode(definition, board, tokens, harbours, robber, pirate, shuffler)));

        if (pluginModes is not null)
            modes.AddRange(pluginModes);

        Modes = modes;
    }

    public GameModeRegistration Default => Modes[0];

    public GameModeRegistration? ByName(string name) =>
        Modes.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
}
