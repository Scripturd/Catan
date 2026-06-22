namespace Catan.Domain.Pieces;

public sealed record BuildingType
{
    public string Name { get; }
    public int Yield { get; }

    public BuildingType(string name, int yield) { Name = name; Yield = yield; }

    public static readonly BuildingType Settlement = new("Settlement", 1);
    public static readonly BuildingType City = new("City", 2);
    public static readonly BuildingType Capital = new("Capital", 3);
}