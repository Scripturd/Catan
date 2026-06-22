namespace Catan.Domain.Board;

/// <summary>
/// The five tradeable resources. This is the core economic vocabulary of the
/// game: it appears in player hands, build costs, bank/port trades, and the
/// Year of Plenty / Monopoly development cards.
///
/// Deliberately has NO "None" member. The desert produces nothing, but a
/// resource hand should never be able to hold "nothing" as a value — that
/// concern belongs to <see cref="TerrainType"/>, not here.
/// </summary>
public enum ResourceType
{
    Brick,
    Lumber,
    Wool,
    Grain,
    Ore
}
