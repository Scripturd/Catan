using Catan.Pieces;

namespace Catan.Game;

public sealed record GameServices(
    BoardService Board,
    NumberTokenService Tokens,
    HarbourService Harbours,
    Robber Robber,
    Shuffler Shuffler);