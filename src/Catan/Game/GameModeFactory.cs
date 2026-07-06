using Catan.Pieces;

namespace Catan.Game;

public delegate IGameMode GameModeFactory(
    BoardService board,
    NumberTokenService tokens,
    HarbourService harbours,
    Robber robber,
    Pirate pirate,
    Shuffler shuffler);
