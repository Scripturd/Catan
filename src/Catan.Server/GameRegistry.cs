using System.Collections.Concurrent;
using Catan.Modding;

namespace Catan.Server;

public sealed class GameRegistry
{
    private readonly ConcurrentDictionary<string, ServerGame> _games = new(StringComparer.OrdinalIgnoreCase);

    public ServerGame Create(BoardDefinition definition, string hostConnectionId)
    {
        var id = NewId();
        var game = new ServerGame(id, definition, hostConnectionId);
        _games[id] = game;
        return game;
    }

    public bool TryGet(string id, out ServerGame game) => _games.TryGetValue(id ?? string.Empty, out game!);

    private string NewId()
    {
        string id;
        do
        {
            id = Guid.NewGuid().ToString("N")[..5].ToUpperInvariant();
        }
        while (_games.ContainsKey(id));

        return id;
    }
}
