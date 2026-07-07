# Game modes

A game mode lays out the board and owns any custom rules. Every mode is a C#
class implementing `Catan.Game.IGameMode`, grouped into an `IExpansionPack`, and
living in its own project. The three shipped modes work this way:
`Catan.Standard`, `Catan.Seafarers`, and `Catan.Mini`. Each is
an ordinary project referenced by the hosts (CLI + server) and registered
explicitly.

> **Note.** Earlier iterations supported two extra ways to add a mode: runtime
> **plugin DLLs** (`AssemblyLoadContext` scanning a `plugins/` folder) and
> **JSON board definitions** (`modes/*.json` loaded at runtime). Both were
> removed — the game ships a fixed set of built-in C# modes, with no
> user-supplied content. If a mode-authoring path is ever wanted again, this is
> where it would be reintroduced.

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
    Robber Robber, Shuffler Shuffler);
```

Because services arrive through `Start` (not the constructor), a mode holds no
per-game state and one instance is reused across games — keep `Start` writing
only to the passed `services`.

## Writing one (worked example: `Catan.Mini`)

1. Create a class library targeting the same framework as the hosts and
   reference the core `Catan.Core` project:

   ```xml
   <ProjectReference Include="..\Catan.Core\Catan.Core.csproj" />
   ```

2. Implement your `IGameMode` (see [MiniGame.cs](src/Catan.Mini/MiniGame.cs))
   and an `IExpansionPack` that exposes it (see
   [MiniPack.cs](src/Catan.Mini/MiniPack.cs)).

3. Add the project to the solution ([Catan.slnx](Catan.slnx)), reference it from
   the hosts that should offer it — [Catan.Cli](src/Catan.Cli/Catan.Cli.csproj)
   and [Catan.Server](src/Catan.Server/Catan.Server.csproj) — and add the pack to
   the built-in list in each host's composition (`src/Catan.Cli/Program.cs` and
   `src/Catan.Server/Program.cs`):

   ```csharp
   IEnumerable<IGameMode> builtIns =
       new IExpansionPack[] { new StandardPack(), new SeafarersPack(), new MiniPack() }
           .SelectMany(pack => pack.Modes);
   var catalog = new ModeCatalog(builtIns);
   ```

## How it works

`ModeCatalog` is a thin container over the built-in modes handed to it by each
host's composition root. The lobby lists everything in `catalog.Modes`; `ByName`
resolves a chosen mode; `Default` is the first. Building any project builds
everything it references, so `dotnet build Catan.slnx` (or running either host)
is all that's needed.
