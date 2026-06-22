using Catan.Domain.Players;

namespace Catan.Domain.Pieces;

public sealed record Building
{
    public PlayerId Owner { get; }
    public BuildingKind Kind { get; }

    public Building(PlayerId owner, BuildingKind kind)
    {
        Owner = owner;
        Kind = kind;
    }
}