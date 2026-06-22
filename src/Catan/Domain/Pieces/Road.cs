using Catan.Domain.Players;

namespace Catan.Domain.Pieces;

public class Road
{
    public PlayerId Owner { get; }

    public Road(PlayerId owner)
    {
        Owner = owner;
    }
}