namespace Catan.Application.Ports;

/// <summary>
/// All randomness flows through this port: dice rolls, deck shuffles, the robber's
/// steal. It lives in the Application layer because that's the layer whose use cases
/// need it — the layer that owns the need owns the port.
///
/// A real adapter wraps an unseeded RNG for play; tests inject a seeded or fully
/// scripted implementation, so an entire game replays deterministically.
/// </summary>
public interface IRandom
{
    /// <summary>Rolls a single six-sided die. Returns 1..6 inclusive.</summary>
    int RollDie();

    /// <summary>Returns an int in [0, maxExclusive). Used for deck/steal draws.</summary>
    int Next(int maxExclusive);
}
