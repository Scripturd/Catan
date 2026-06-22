using Catan.Players;

namespace Catan.Pieces;

public class Road
{
    public PlayerId Owner { get; }

    public Road(PlayerId owner)
    {
        Owner = owner;
    }
}