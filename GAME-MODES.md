# Game modes

A game mode lays out the board and owns any custom rules. There are two ways to
add one:

1. **A C# mode** — a class implementing `Catan.Game.IGameMode`, grouped into an
   `IExpansionPack`, living in its own project. The three shipped modes work this
   way: `Catan.Modes.Standard`, `Catan.Modes.Seafarers`, and `Catan.Modes.Mini`.
   Each is an ordinary project referenced by the hosts (CLI + server) and
   registered explicitly — **no runtime plugin loading**.
2. **A JSON board definition** — inert data in `modes/`, loaded at runtime by
   `DataDrivenGameMode`. Best for a new *board* that needs no custom C# rules.

> **Note.** This project used to load modes as runtime plugin DLLs from a
> `plugins/` folder (`AssemblyLoadContext`). That was removed in favour of plain
> project references — the shipped modes are compiled in and appear in the mode
> list with no separate build step. If you later want *third parties* to drop in
> a compiled mode without rebuilding, the plugin loader can be reintroduced; for
> untrusted authors, prefer JSON board definitions, which are data, not code.

## The contract

An expansion pack is any class implementing `Catan.Game.IExpansionPack` that
exposes one or more game modes:

```csharp
public interface IExpansionPack
{
    IEnumerable<IGameMode> Modes { get; }
}
```

Each mode is an `IGameMode`. It carries its own metadata (so the lobby can list
it before a game exists) and lays out the board in `Start`, which receives the
board services and the players:

```csharp
public interface IGameMode
{
    string Name { get; }
    int MinPlayerCount { get; }
    int MaxPlayerCount { get; }
    void Start(GameServices services, IReadOnlyList<PlayerId> players);
}

public sealed record GameServices(
    BoardService Board, NumberTokenService Tokens, HarbourService Harbours,
    Robber Robber, MarkerRegistry Markers, Shuffler Shuffler);
```

Because services arrive through `Start` (not the constructor), a mode holds no
per-game state and one instance is reused across games — keep `Start` writing
only to the passed `services`.

## Writing one (worked example: `Catan.Modes.Mini`)

1. Create a class library targeting the same framework as the hosts and
   reference the core `Catan` project:

   ```xml
   <ProjectReference Include="..\Catan\Catan.csproj" />
   ```

2. Implement your `IGameMode` (see [MiniGame.cs](src/Catan.Modes.Mini/MiniGame.cs))
   and an `IExpansionPack` that exposes it (see
   [MiniPack.cs](src/Catan.Modes.Mini/MiniPack.cs)).

3. Add the project to the solution ([Catan.slnx](Catan.slnx)), reference it from
   the hosts that should offer it — [Catan.Cli](src/Catan.Cli/Catan.Cli.csproj)
   and [Catan.Server](src/Catan.Server/Catan.Server.csproj) — and add the pack to
   the built-in list in each host's composition (`src/Catan.Cli/Program.cs` and
   `src/Catan.Server/Program.cs`):

   ```csharp
   IEnumerable<IGameMode> builtIns =
       new IExpansionPack[] { new StandardPack(), new SeafarersPack(), new MiniPack() }
           .SelectMany(pack => pack.Modes);
   var catalog = new ModeCatalog(Path.Combine(AppContext.BaseDirectory, "modes"), builtIns);
   ```

## How it works

`ModeCatalog` is built once per host from two sources: the JSON board
definitions found in `modes/` (each wrapped in a `DataDrivenGameMode`) and the
built-in modes handed to it by the composition root. The lobby lists everything
in `catalog.Modes`; `ByName` resolves a chosen mode; `Default` is the first.
Building any project builds everything it references, so `dotnet build
Catan.slnx` (or running either host) is all that's needed.
