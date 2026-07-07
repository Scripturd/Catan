namespace Catan.Server;

public sealed record StateSnapshot(
    string GameId,
    string Phase,
    IReadOnlyList<PlayerView> Players,
    int? CurrentPlayerId,
    int MinPlayers,
    int MaxPlayers,
    BoardView? Board);

public sealed record PlayerView(int Id, string Name, string Color, IReadOnlyDictionary<string, int>? Hand);

public sealed record BoardView(
    double MinX,
    double MinY,
    double Width,
    double Height,
    IReadOnlyList<HexView> Hexes,
    IReadOnlyList<HarbourView> Harbours,
    PointView? Robber,
    IReadOnlyList<VertexView> Vertices,
    IReadOnlyList<EdgeView> Edges,
    IReadOnlyList<BuildingView> Settlements,
    IReadOnlyList<RoadView> Roads);

public sealed record HexView(int Q, int R, string Terrain, string Color, int? Token, double X, double Y);

public sealed record HarbourView(int Ratio, string? Resource, string Color, double X, double Y);

public sealed record PointView(double X, double Y);

public sealed record VertexView(int Q, int R, string Corner, double X, double Y);

public sealed record EdgeView(int Q, int R, string Direction, double X1, double Y1, double X2, double Y2);

public sealed record BuildingView(int Q, int R, string Corner, double X, double Y, int Owner, string Color);

public sealed record RoadView(int Q, int R, string Direction, double X1, double Y1, double X2, double Y2, int Owner, string Color);

public sealed record VertexDto(int Q, int R, string Corner);

public sealed record EdgeDto(int Q, int R, string Direction);
