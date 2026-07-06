using Catan.Geometry;
using Catan.GameModes;
using Microsoft.AspNetCore.SignalR;

namespace Catan.Server;

public sealed record JoinInfo(string GameId, int PlayerId);

public sealed class GameHub : Hub
{
    private readonly GameRegistry _registry;
    private readonly ModeCatalog _catalog;

    public GameHub(GameRegistry registry, ModeCatalog catalog)
    {
        _registry = registry;
        _catalog = catalog;
    }

    public IEnumerable<string> GetModes() => _catalog.Modes.Select(m => m.Name);

    public async Task<JoinInfo> CreateGame(string playerName, string? modeName)
    {
        var mode = (modeName is null ? null : _catalog.ByName(modeName)) ?? _catalog.Default;
        var game = _registry.Create(mode, Context.ConnectionId);
        var (_, _, playerId) = game.AddPlayer(Context.ConnectionId, playerName);

        await Groups.AddToGroupAsync(Context.ConnectionId, game.Id);
        await Broadcast(game);
        return new JoinInfo(game.Id, playerId);
    }

    public async Task<JoinInfo> JoinGame(string gameId, string playerName)
    {
        var game = Require(gameId);
        var (ok, error, playerId) = game.AddPlayer(Context.ConnectionId, playerName);
        if (!ok)
            throw new HubException(error!);

        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        await Broadcast(game);
        return new JoinInfo(gameId, playerId);
    }

    public async Task StartGame(string gameId)
    {
        var game = Require(gameId);
        var result = game.Start();
        if (!result.Success)
            throw new HubException(result.Error!);

        await Broadcast(game);
    }

    public async Task PlaceStarting(string gameId, VertexDto settlement, EdgeDto road)
    {
        var game = Require(gameId);

        Vertex vertex;
        Edge edge;
        try
        {
            vertex = new Vertex(settlement.Q, settlement.R, Enum.Parse<VertexCorner>(settlement.Corner, ignoreCase: true));
            edge = new Edge(road.Q, road.R, Enum.Parse<EdgeDirection>(road.Direction, ignoreCase: true));
        }
        catch
        {
            throw new HubException("That placement is malformed.");
        }

        var result = game.PlaceStarting(Context.ConnectionId, vertex, edge);
        if (!result.Success)
            throw new HubException(result.Error!);

        await Broadcast(game);
    }

    private ServerGame Require(string gameId)
    {
        if (!_registry.TryGet(gameId, out var game))
            throw new HubException("No game exists with that code.");
        return game;
    }

    private Task Broadcast(ServerGame game) =>
        Clients.Group(game.Id).SendAsync("State", game.Snapshot());
}