using Catan.Game;
using Catan.Geometry;
using Catan.Players;

namespace Catan.Server;

public sealed record LobbyPlayer(PlayerId Id, string Name, string Color, string ConnectionId);

public sealed class ServerGame
{
    private readonly object _gate = new();
    private readonly List<LobbyPlayer> _players = [];

    public string Id { get; }
    public GameModeRegistration Mode { get; }
    public string HostConnectionId { get; }
    public GameSession? Session { get; private set; }

    public ServerGame(string id, GameModeRegistration mode, string hostConnectionId)
    {
        Id = id;
        Mode = mode;
        HostConnectionId = hostConnectionId;
    }

    public (bool Success, string? Error, int PlayerId) AddPlayer(string connectionId, string name)
    {
        lock (_gate)
        {
            if (Session is not null)
                return (false, "The game has already started.", -1);
            if (_players.Count >= Mode.MaxPlayers)
                return (false, "The game is full.", -1);

            var existing = _players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (existing is not null)
                return (true, null, existing.Id.Value);

            var id = _players.Count;
            var displayName = string.IsNullOrWhiteSpace(name) ? $"Player {id + 1}" : name.Trim();
            _players.Add(new LobbyPlayer(new PlayerId(id), displayName, PlayerColors.For(id), connectionId));
            return (true, null, id);
        }
    }

    public MoveResult Start()
    {
        lock (_gate)
        {
            if (Session is not null)
                return MoveResult.Rejected("The game has already started.");
            if (_players.Count < Mode.MinPlayers)
                return MoveResult.Rejected($"At least {Mode.MinPlayers} players are needed to start.");

            var ids = _players.Select(p => p.Id).ToList();
            Session = new GameSession(Mode.Build, ids, Random.Shared);
            return MoveResult.Accepted();
        }
    }

    public MoveResult PlaceStarting(string connectionId, Vertex settlement, Edge road)
    {
        lock (_gate)
        {
            if (Session is null)
                return MoveResult.Rejected("The game has not started yet.");

            var player = _players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player is null)
                return MoveResult.Rejected("You are not a player in this game.");

            return Session.PlaceStartingSettlementAndRoad(player.Id, settlement, road);
        }
    }

    public StateSnapshot Snapshot()
    {
        lock (_gate)
            return SnapshotBuilder.Build(Id, Mode, _players, Session);
    }
}
