namespace Catan.Domain.Pieces;

/// <summary>
/// A kind of building. Differs only by data (yield, and later cost), so it's a value,
/// not a class hierarchy — a new kind like Capital is just another entry.
/// </summary>
public sealed record BuildingType(string Name, int Yield)
{
    public static readonly BuildingType Settlement = new("Settlement", 1);
    public static readonly BuildingType City = new("City", 2);
    public static readonly BuildingType Capital = new("Capital", 3);
}
