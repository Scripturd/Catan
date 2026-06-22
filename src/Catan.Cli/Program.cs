using Catan.Domain.Board;

// The CLI is the console runner. For now it just proves the app builds and can reach
// the domain. The real game loop grows here.
Console.WriteLine("Catan (pure .NET) — terrain -> resource");
Console.WriteLine();

foreach (var terrain in Enum.GetValues<TerrainType>())
{
    var produces = terrain.Produces();
    var label = produces is { } resource ? resource.ToString() : "nothing";
    Console.WriteLine($"  {terrain,-10} -> {label}");
}
