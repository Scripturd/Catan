using System.Collections.Generic;
using Catan.Application.Ports;

namespace Catan.Application.Tests.TestDoubles;

/// <summary>
/// Deterministic <see cref="IRandom"/> for tests: hands back queued values in order.
/// This is the adapter that makes "replay an entire game" possible.
/// </summary>
internal sealed class QueuedRandom(params int[] values) : IRandom
{
    private readonly Queue<int> _values = new(values);

    public int RollDie() => _values.Dequeue();

    public int Next(int maxExclusive) => _values.Dequeue();
}
