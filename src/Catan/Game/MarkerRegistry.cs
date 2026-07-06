using Catan.Pieces;

namespace Catan.Game;

public sealed class MarkerRegistry
{
    private readonly List<Marker> _markers = [];

    public IReadOnlyList<Marker> All => _markers;

    public void Place(Marker marker) => _markers.Add(marker);

    public void Clear() => _markers.Clear();
}
