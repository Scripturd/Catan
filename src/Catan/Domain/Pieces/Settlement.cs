using Catan.Domain.Players;

namespace Catan.Domain.Pieces;

public class Settlement
{
    public PlayerId Owner { get; }

    public Settlement(PlayerId owner)
    {
        Owner = owner;
    }
}