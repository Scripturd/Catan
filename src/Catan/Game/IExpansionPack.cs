namespace Catan.Game;

public interface IExpansionPack
{
    IEnumerable<GameModeRegistration> Modes { get; }
}
