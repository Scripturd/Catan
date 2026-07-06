namespace Catan.Game;

public interface IGameModePlugin
{
    IEnumerable<GameModeRegistration> Modes { get; }
}
