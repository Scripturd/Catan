# Game-mode plugins

A game mode lives in its own assembly (DLL) and is loaded by the server (and the
CLI) at runtime — no rebuild required. Drop a plugin DLL in the `plugins` folder
next to the app and its modes appear in the mode list. There are **no built-in
modes compiled into core** — every C# mode is a plugin. The three shipped modes
are each their own project: `Catan.Modes.Standard`, `Catan.Modes.Seafarers`, and
`Catan.Modes.Mini`. (JSON board definitions in `modes/` are the other, safe way
to add a mode — see the trust warning.)

> ⚠️ **Trust warning.** A plugin is compiled C# that runs **with full trust in
> the server process** — it can do anything the server can (read/write files,
> open network connections, etc.). Only install plugins you wrote or trust. For
> letting *untrusted* people add content, prefer **JSON board definitions**
> (see `modes/`), which are inert data, not code. Plugins are for modes with
> custom C# *rules* that JSON can't express.

## The contract

An expansion pack is any public class with a parameterless constructor that
implements `Catan.Game.IExpansionPack` and exposes one or more game modes. A
plugin DLL provides one or more expansion packs; the loader discovers them all.

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
    Robber Robber, Pirate Pirate, Shuffler Shuffler);
```

Because services arrive through `Start` (not the constructor), a mode holds no
per-game state and one instance is reused across games — keep `Start` writing
only to the passed `services`.

## Writing one (worked example: `Catan.Modes.Mini`)

1. Create a class library targeting the same framework as the server.
2. Reference the core `Catan` project **with `Private="false"`** so the plugin
   does **not** ship its own copy of `Catan.dll` — the host provides it, and
   that shared identity is what lets the cast to `IExpansionPack` succeed:

   ```xml
   <ProjectReference Include="..\Catan\Catan.csproj" Private="false" />
   ```

3. Implement your `IGameMode` (see [MiniGame.cs](src/Catan.Modes.Mini/MiniGame.cs))
   and an `IExpansionPack` that registers it (see
   [MiniPack.cs](src/Catan.Modes.Mini/MiniPack.cs)).

## Installing

The app scans `<app-output>/plugins/*.dll` at startup, loads each in its own
`AssemblyLoadContext`, and adds every `GameModeRegistration` it finds. A DLL that
fails to load is logged and skipped rather than crashing the app.

- **Server:** the shipped plugins are wired in via [build/Plugins.targets](build/Plugins.targets)
  (referenced with `ReferenceOutputAssembly=false`, so they're built and copied
  into the output `plugins/` folder without becoming a compile-time dependency).
  `dotnet run --project src/Catan.Server` just works. The [Dockerfile](Dockerfile)
  copies them into `/app/plugins` for deployment.
- **CLI:** because the CLI lives in the core assembly (which the mode projects
  reference), it can't build the plugins itself. Run **`./build.ps1`** to build
  everything and assemble the `plugins/` folders for both the CLI and server.
- **A new plugin:** `dotnet build src/<YourPlugin> -c Release`, then copy its DLL
  into the app's `plugins/` folder (or add it to `build/Plugins.targets`).

## How it works

`PluginLoader` loads each DLL through a `PluginLoadContext` (a custom
`AssemblyLoadContext`) that resolves the shared `Catan` assembly from the host
instead of the plugin folder, so the plugin's types and the host's contract
types have the same identity. It then finds `IExpansionPack` implementations,
instantiates them, and collects their registrations into the `ModeCatalog`
alongside the JSON board definitions.
