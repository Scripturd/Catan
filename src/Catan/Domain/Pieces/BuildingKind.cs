namespace Catan.Domain.Pieces;

public sealed record BuildingKind
{
    public string Name { get; }
    public int Yield { get; }

    public static readonly BuildingKind Settlement = new("Settlement", 1);
    public static readonly BuildingKind City = new("City", 2);

    public BuildingKind(string name, int yield) 
    { 
        Name = name; 
        Yield = yield; 
    }
}