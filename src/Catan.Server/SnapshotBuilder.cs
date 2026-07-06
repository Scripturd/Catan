using Catan.Game;
using Catan.Geometry;

namespace Catan.Server;

public static class SnapshotBuilder
{
    public static StateSnapshot Build(string id, GameModeRegistration mode, IReadOnlyList<LobbyPlayer> players, GameSession? session)
    {
        var phase = session is null ? "Lobby" : session.SetupComplete ? "Complete" : "Setup";

        var playerViews = players
            .Select(p => new PlayerView(
                p.Id.Value,
                p.Name,
                p.Color,
                session is null ? null : ToResourceView(session.Resources.Of(p.Id))))
            .ToList();

        var board = session is null ? null : BuildBoard(session, players);

        return new StateSnapshot(
            id,
            phase,
            playerViews,
            session?.CurrentPlayer?.Value,
            mode.MinPlayers,
            mode.MaxPlayers,
            board);
    }

    private static BoardView BuildBoard(GameSession session, IReadOnlyList<LobbyPlayer> players)
    {
        var colorByPlayer = players.ToDictionary(p => p.Id.Value, p => p.Color);
        var centres = session.Board.Hexes.ToDictionary(h => h, BoardLayout.HexCentre);

        double minX = centres.Values.Min(c => c.X) - BoardLayout.Size - BoardLayout.Margin;
        double minY = centres.Values.Min(c => c.Y) - BoardLayout.Size - BoardLayout.Margin;
        double maxX = centres.Values.Max(c => c.X) + BoardLayout.Size + BoardLayout.Margin;
        double maxY = centres.Values.Max(c => c.Y) + BoardLayout.Size + BoardLayout.Margin;

        double centroidX = centres.Values.Average(c => c.X);
        double centroidY = centres.Values.Average(c => c.Y);

        var hexes = session.Board.Hexes
            .Select(h =>
            {
                var (x, y) = centres[h];
                return new HexView(h.Q, h.R, session.Board.TerrainAt(h).ToString(), session.Tokens.At(h)?.Number, x, y);
            })
            .ToList();

        var harbours = session.Harbours.All
            .Select(entry =>
            {
                var (a, b) = session.Board.EndpointsOf(entry.Key);
                var (ax, ay) = BoardLayout.VertexPixel(a);
                var (bx, by) = BoardLayout.VertexPixel(b);
                double mx = (ax + bx) / 2;
                double my = (ay + by) / 2;
                double dx = mx - centroidX;
                double dy = my - centroidY;
                double length = Math.Max(Math.Sqrt(dx * dx + dy * dy), 0.0001);
                double hx = mx + dx / length * BoardLayout.Size * 0.4;
                double hy = my + dy / length * BoardLayout.Size * 0.4;
                return new HarbourView(entry.Value.Ratio, entry.Value.Resource?.ToString(), hx, hy);
            })
            .ToList();

        var vertices = session.Board.Vertices
            .Select(v =>
            {
                var (x, y) = BoardLayout.VertexPixel(v);
                return new VertexView(v.Q, v.R, v.Corner.ToString(), x, y);
            })
            .ToList();

        var edges = session.Board.Edges
            .Select(e =>
            {
                var (a, b) = session.Board.EndpointsOf(e);
                var (x1, y1) = BoardLayout.VertexPixel(a);
                var (x2, y2) = BoardLayout.VertexPixel(b);
                return new EdgeView(e.Q, e.R, e.Direction.ToString(), x1, y1, x2, y2);
            })
            .ToList();

        var settlements = session.Settlements.All
            .Select(entry =>
            {
                var (x, y) = BoardLayout.VertexPixel(entry.Key);
                int owner = entry.Value.Owner.Value;
                return new BuildingView(entry.Key.Q, entry.Key.R, entry.Key.Corner.ToString(), x, y, owner, colorByPlayer[owner]);
            })
            .ToList();

        var roads = session.Roads.All
            .Select(entry =>
            {
                var (a, b) = session.Board.EndpointsOf(entry.Key);
                var (x1, y1) = BoardLayout.VertexPixel(a);
                var (x2, y2) = BoardLayout.VertexPixel(b);
                int owner = entry.Value.Owner.Value;
                return new RoadView(entry.Key.Q, entry.Key.R, entry.Key.Direction.ToString(), x1, y1, x2, y2, owner, colorByPlayer[owner]);
            })
            .ToList();

        PointView? robber = session.Robber.IsPlaced ? ToPoint(BoardLayout.HexCentre(session.Robber.Hex)) : null;
        PointView? pirate = session.Pirate.IsPlaced ? ToPoint(BoardLayout.HexCentre(session.Pirate.Hex)) : null;

        return new BoardView(minX, minY, maxX - minX, maxY - minY, hexes, harbours, robber, pirate, vertices, edges, settlements, roads);
    }

    private static PointView ToPoint((double X, double Y) p) => new(p.X, p.Y);

    private static ResourceView ToResourceView(Catan.Economy.ResourceBag bag) =>
        new(bag.Brick, bag.Lumber, bag.Wool, bag.Grain, bag.Ore);
}
