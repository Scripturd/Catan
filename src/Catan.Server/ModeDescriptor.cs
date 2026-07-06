using Catan.Game;

namespace Catan.Server;

public sealed record ModeDescriptor(string Name, int MinPlayers, int MaxPlayers, GameModeFactory Build);
