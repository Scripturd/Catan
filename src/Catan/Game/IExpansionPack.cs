namespace Catan.Game;

public interface IExpansionPack
{
    IEnumerable<IGameMode> Modes { get; }
}