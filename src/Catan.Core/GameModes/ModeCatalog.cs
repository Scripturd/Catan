using Catan.Game;

namespace Catan.GameModes;

public sealed class ModeCatalog
{
    public IReadOnlyList<IGameMode> Modes { get; }

    public ModeCatalog(IEnumerable<IGameMode> modes)
    {
        Modes = modes.ToList();
    }

    public IGameMode Default => Modes[0];

    public IGameMode? ByName(string name) =>
        Modes.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
}