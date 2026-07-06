using Catan.Economy;
using Catan.Players;

namespace Catan.Pieces;

public class Road
{
    public static readonly ResourceBag Cost = new((ResourceType.Brick, 1), (ResourceType.Lumber, 1));

    public PlayerId Owner { get; }

    public Road(PlayerId owner)
    {
        Owner = owner;
    }
}