using Catan.Pieces;

namespace Catan.Game;

public sealed record GameServices(
    BoardService Board,
    NumberTokenService Tokens,
    HarbourService Harbours,
    Robber Robber,
    MarkerRegistry Markers,
    Shuffler Shuffler);
