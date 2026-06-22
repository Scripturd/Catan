namespace Catan.Application.Features.Turn;

/// <summary>The public contract of the Turn feature.</summary>
public interface IStartTurn
{
    TurnStart Execute(StartTurnCommand command);
}
