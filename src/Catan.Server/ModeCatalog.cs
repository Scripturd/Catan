using Catan.Modding;

namespace Catan.Server;

public sealed class ModeCatalog
{
    public IReadOnlyList<BoardDefinition> Modes { get; }

    public ModeCatalog(string directory)
    {
        Modes = new BoardDefinitionLoader().LoadDirectory(directory);
    }

    public BoardDefinition Default => Modes[0];

    public BoardDefinition? ByName(string name) =>
        Modes.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
}
