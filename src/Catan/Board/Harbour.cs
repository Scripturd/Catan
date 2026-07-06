using Catan.Economy;

namespace Catan.Board;

public readonly record struct Harbour
{
    public int Ratio { get; }
    public ResourceType? Resource { get; }

    public Harbour(int ratio, ResourceType? resource)
    {
        Ratio = ratio;
        Resource = resource;
    }
    public Harbour(int ratio)
    {
        Ratio = ratio;
    }
}