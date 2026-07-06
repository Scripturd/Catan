using Catan.Economy;
using Catan.Game;
using Catan.Pieces;
using Catan.Players;

namespace Catan.Modding;

public sealed class DataDrivenGameMode : IGameMode
{
    private readonly BoardDefinition _definition;
    private readonly BoardService _boardService;
    private readonly NumberTokenService _numberTokenService;
    private readonly HarbourService _harbourService;
    private readonly Robber _robber;
    private readonly Pirate _pirate;
    private readonly Shuffler _shuffler;

    public int MinPlayerCount => _definition.MinPlayers;
    public int MaxPlayerCount => _definition.MaxPlayers;

    public DataDrivenGameMode(
        BoardDefinition definition,
        BoardService boardService,
        NumberTokenService numberTokenService,
        HarbourService harbourService,
        Robber robber,
        Pirate pirate,
        Shuffler shuffler)
    {
        _definition = definition;
        _boardService = boardService;
        _numberTokenService = numberTokenService;
        _harbourService = harbourService;
        _robber = robber;
        _pirate = pirate;
        _shuffler = shuffler;
    }

    public void Start(IReadOnlyList<PlayerId> players)
    {
        Cleanup();

        var landHexes = PlaceHexes();
        PlaceTokens(landHexes);
        PlaceHarbours();
        PlaceRobber();
        PlacePirate();
    }

    private void Cleanup()
    {
        _boardService.Clear();
        _numberTokenService.Clear();
        _harbourService.Clear();
        _robber.Remove();
        _pirate.Remove();
    }

    private IReadOnlyList<Hex> PlaceHexes()
    {
        var landDefinitions = _definition.Hexes.Where(h => h.Terrain != TerrainType.Sea).ToList();
        var seaDefinitions = _definition.Hexes.Where(h => h.Terrain == TerrainType.Sea);

        foreach (var sea in seaDefinitions)
            _boardService.AddHex(new Hex(sea.Q, sea.R), TerrainType.Sea);

        var terrains = landDefinitions.Select(h => h.Terrain).ToList();
        if (_definition.RandomizeTerrain)
            terrains = _shuffler.Shuffle(terrains);

        var landHexes = new List<Hex>(landDefinitions.Count);
        for (int i = 0; i < landDefinitions.Count; i++)
        {
            var hex = new Hex(landDefinitions[i].Q, landDefinitions[i].R);
            _boardService.AddHex(hex, terrains[i]);
            landHexes.Add(hex);
        }

        return landHexes;
    }

    private void PlaceTokens(IReadOnlyList<Hex> landHexes)
    {
        var tokens = _definition.Hexes
            .Where(h => h.Token.HasValue)
            .Select(h => h.Token!.Value)
            .ToList();
        if (_definition.RandomizeTokens)
            tokens = _shuffler.Shuffle(tokens);

        var productiveHexes = landHexes
            .Where(hex => TerrainYields.For(_boardService.TerrainAt(hex)) != Yield.Nothing)
            .ToList();

        for (int i = 0; i < productiveHexes.Count; i++)
            _numberTokenService.Place(productiveHexes[i], new NumberToken(tokens[i]));
    }

    private void PlaceHarbours()
    {
        foreach (var harbour in _definition.Harbours)
        {
            var edge = new Edge(harbour.Edge.Q, harbour.Edge.R, harbour.Edge.Direction);
            _harbourService.Place(edge, ToHarbour(harbour));
        }
    }

    private static Harbour ToHarbour(HarbourDefinition harbour) =>
        harbour.Resource.HasValue
            ? new Harbour(harbour.Ratio, harbour.Resource.Value)
            : new Harbour(harbour.Ratio);

    private void PlaceRobber()
    {
        if (_definition.Robber is { } robber)
        {
            _robber.Place(new Hex(robber.Q, robber.R));
            return;
        }

        var desert = _boardService.HexesOf(TerrainType.Desert).Cast<Hex?>().FirstOrDefault();
        if (desert is { } hex)
            _robber.Place(hex);
    }

    private void PlacePirate()
    {
        if (_definition.Pirate is { } pirate)
            _pirate.Place(new Hex(pirate.Q, pirate.R));
    }

    public override string ToString() => _definition.Name;
}
