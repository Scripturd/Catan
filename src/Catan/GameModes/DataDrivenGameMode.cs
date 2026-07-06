using Catan.Economy;
using Catan.Game;
using Catan.Players;

namespace Catan.GameModes;

public sealed class DataDrivenGameMode : IGameMode
{
    private readonly BoardDefinition _definition;

    public string Name => _definition.Name;
    public int MinPlayerCount => _definition.MinPlayers;
    public int MaxPlayerCount => _definition.MaxPlayers;

    public DataDrivenGameMode(BoardDefinition definition)
    {
        _definition = definition;
    }

    public void Start(GameServices services, IReadOnlyList<PlayerId> players)
    {
        var landHexes = PlaceHexes(services);
        PlaceTokens(services, landHexes);
        PlaceHarbours(services);
        PlaceRobber(services);
        PlacePirate(services);
    }

    private IReadOnlyList<Hex> PlaceHexes(GameServices services)
    {
        var landDefinitions = _definition.Hexes.Where(h => h.Terrain != TerrainType.Sea).ToList();
        var seaDefinitions = _definition.Hexes.Where(h => h.Terrain == TerrainType.Sea);

        foreach (var sea in seaDefinitions)
            services.Board.AddHex(new Hex(sea.Q, sea.R), TerrainType.Sea);

        var terrains = landDefinitions.Select(h => h.Terrain).ToList();
        if (_definition.RandomizeTerrain)
            terrains = services.Shuffler.Shuffle(terrains);

        var landHexes = new List<Hex>(landDefinitions.Count);
        for (int i = 0; i < landDefinitions.Count; i++)
        {
            var hex = new Hex(landDefinitions[i].Q, landDefinitions[i].R);
            services.Board.AddHex(hex, terrains[i]);
            landHexes.Add(hex);
        }

        return landHexes;
    }

    private void PlaceTokens(GameServices services, IReadOnlyList<Hex> landHexes)
    {
        var tokens = _definition.Hexes
            .Where(h => h.Token.HasValue)
            .Select(h => h.Token!.Value)
            .ToList();
        if (_definition.RandomizeTokens)
            tokens = services.Shuffler.Shuffle(tokens);

        var productiveHexes = landHexes
            .Where(hex => TerrainYields.For(services.Board.TerrainAt(hex)) != Yield.Nothing)
            .ToList();

        for (int i = 0; i < productiveHexes.Count; i++)
            services.Tokens.Place(productiveHexes[i], new NumberToken(tokens[i]));
    }

    private void PlaceHarbours(GameServices services)
    {
        foreach (var harbour in _definition.Harbours)
        {
            var edge = new Edge(harbour.Edge.Q, harbour.Edge.R, harbour.Edge.Direction);
            services.Harbours.Place(edge, ToHarbour(harbour));
        }
    }

    private static Harbour ToHarbour(HarbourDefinition harbour) =>
        harbour.Resource.HasValue
            ? new Harbour(harbour.Ratio, harbour.Resource.Value)
            : new Harbour(harbour.Ratio);

    private void PlaceRobber(GameServices services)
    {
        if (_definition.Robber is { } robber)
        {
            services.Robber.Place(new Hex(robber.Q, robber.R));
            return;
        }

        var desert = services.Board.HexesOf(TerrainType.Desert).Cast<Hex?>().FirstOrDefault();
        if (desert is { } hex)
            services.Robber.Place(hex);
    }

    private void PlacePirate(GameServices services)
    {
        if (_definition.Pirate is { } pirate)
            services.Pirate.Place(new Hex(pirate.Q, pirate.R));
    }
}
