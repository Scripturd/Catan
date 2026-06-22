namespace Catan.Domain.Board;

public sealed class Edge
{
    public EdgeId Id { get; }
    public VertexId A { get; }
    public VertexId B { get; }

    public Edge(EdgeId id, VertexId a, VertexId b)
    {
        Id = id;
        A = a;
        B = b;
    }
}