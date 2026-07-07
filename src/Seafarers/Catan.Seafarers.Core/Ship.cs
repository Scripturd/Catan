using Catan.Economy;
using Catan.Players;

namespace Catan.Seafarers;

public class Ship
{
    public static readonly ResourceBag Cost = new((ResourceType.Lumber, 1), (ResourceType.Wool, 1));

    public PlayerId Owner { get; }

    public Ship(PlayerId owner)
    {
        Owner = owner;
    }
}
