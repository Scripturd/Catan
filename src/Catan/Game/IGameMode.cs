using Catan.Players;

namespace Catan.Game;

public interface IGameMode
{
    public int MinPlayerCount { get; }
    public int MaxPlayerCount { get; }
    public void Start(IReadOnlyList<PlayerId> players);
}