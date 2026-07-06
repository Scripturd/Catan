namespace Catan.Game;

public sealed record GameModeRegistration(string Name, int MinPlayers, int MaxPlayers, GameModeFactory Build);