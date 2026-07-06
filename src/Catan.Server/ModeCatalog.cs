using Catan.Modding;
using Catan.SeafarersScenario1;
using Catan.Standard;

namespace Catan.Server;

public sealed class ModeCatalog
{
    public IReadOnlyList<ModeDescriptor> Modes { get; }

    public ModeCatalog(string directory)
    {
        var modes = new List<ModeDescriptor>
        {
            new("Standard Catan", 3, 4,
                (board, tokens, harbours, robber, _, shuffler) =>
                    new StandardGame(board, tokens, harbours, robber, shuffler)),
            new("Seafarers: Heading for New Shores", 3, 4,
                (board, tokens, harbours, robber, pirate, shuffler) =>
                    new SeafarersScenario1Game(board, tokens, harbours, robber, pirate, shuffler)),
        };

        foreach (var definition in new BoardDefinitionLoader().LoadDirectory(directory))
            modes.Add(new ModeDescriptor(definition.Name, definition.MinPlayers, definition.MaxPlayers,
                (board, tokens, harbours, robber, pirate, shuffler) =>
                    new DataDrivenGameMode(definition, board, tokens, harbours, robber, pirate, shuffler)));

        Modes = modes;
    }

    public ModeDescriptor Default => Modes[0];

    public ModeDescriptor? ByName(string name) =>
        Modes.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
}
