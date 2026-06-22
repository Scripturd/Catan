using Catan.Application.Ports;

namespace Catan.Cli.Adapters;

/// <summary>
/// Production <see cref="IRandom"/> adapter backed by <see cref="System.Random"/>.
/// Optionally seeded so a session can be replayed for debugging.
/// </summary>
public sealed class SystemRandom : IRandom
{
    private readonly Random _random;

    public SystemRandom(int? seed = null) =>
        _random = seed is { } s ? new Random(s) : new Random();

    public int RollDie() => _random.Next(1, 7); // upper bound is exclusive

    public int Next(int maxExclusive) => _random.Next(maxExclusive);
}
