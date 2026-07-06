using Catan.Players;

namespace Catan.Game;

public interface IGameMode
{
    string Name { get; }
    int MinPlayerCount { get; }
    int MaxPlayerCount { get; }
    void Start(GameServices services, IReadOnlyList<PlayerId> players);
}
