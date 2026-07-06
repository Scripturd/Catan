namespace Catan.Board;

public sealed class HarbourService
{
    private readonly BoardService _board;
    private readonly Dictionary<Edge, Harbour> _harbours = [];

    public HarbourService(BoardService board)
    {
        _board = board;
    }

    public void Place(Edge edge, Harbour harbour)
    {
        if (!_board.IsCoastal(edge))
            throw new InvalidOperationException($"A harbour must sit on a coastal edge, but {edge} is not coastal.");

        _harbours.Add(edge, harbour);
    }

    public void Clear() => _harbours.Clear();

    public Harbour? At(Edge edge) => _harbours.TryGetValue(edge, out var harbour) ? harbour : null;

    public IReadOnlyDictionary<Edge, Harbour> All => _harbours;

    public IEnumerable<Harbour> AccessibleFrom(Vertex vertex) =>
        _board.EdgesAround(vertex)
            .Where(_harbours.ContainsKey)
            .Select(edge => _harbours[edge]);
}