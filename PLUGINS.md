# Game-mode plugins

A game mode can live in its own assembly (DLL) and be loaded by the server at
runtime — no rebuild required. Drop a plugin DLL in the server's `plugins`
folder and its modes appear in the game's mode list next to the built-ins.

> ⚠️ **Trust warning.** A plugin is compiled C# that runs **with full trust in
> the server process** — it can do anything the server can (read/write files,
> open network connections, etc.). Only install plugins you wrote or trust. For
> letting *untrusted* people add content, prefer **JSON board definitions**
> (see `modes/`), which are inert data, not code. Plugins are for modes with
> custom C# *rules* that JSON can't express.

## The contract

A plugin is any public class with a parameterless constructor that implements
`Catan.Game.IGameModePlugin`:

```csharp
public interface IGameModePlugin
{
    IEnumerable<GameModeRegistration> Modes { get; }
}

public sealed record GameModeRegistration(
    string Name, int MinPlayers, int MaxPlayers, GameModeFactory Build);
```

`GameModeFactory` receives the game's services and returns an `IGameMode`:

```csharp
public delegate IGameMode GameModeFactory(
    BoardService board, NumberTokenService tokens, HarbourService harbours,
    Robber robber, Pirate pirate, Shuffler shuffler);
```

## Writing one (worked example: `Catan.Modes.Mini`)

1. Create a class library targeting the same framework as the server.
2. Reference the core `Catan` project **with `Private="false"`** so the plugin
   does **not** ship its own copy of `Catan.dll` — the host provides it, and
   that shared identity is what lets the cast to `IGameModePlugin` succeed:

   ```xml
   <ProjectReference Include="..\Catan\Catan.csproj" Private="false" />
   ```

3. Implement your `IGameMode` (see [MiniGame.cs](src/Catan.Modes.Mini/MiniGame.cs))
   and an `IGameModePlugin` that registers it (see
   [MiniPlugin.cs](src/Catan.Modes.Mini/MiniPlugin.cs)).

## Installing

```bash
dotnet build src/Catan.Modes.Mini -c Release
# copy the plugin DLL next to the running server, under a "plugins" folder:
cp src/Catan.Modes.Mini/bin/Release/net9.0/Catan.Modes.Mini.dll <server-output>/plugins/
```

The server scans `<server-output>/plugins/*.dll` at startup, loads each in its
own `AssemblyLoadContext`, and adds every `GameModeRegistration` it finds. A DLL
that fails to load is logged and skipped rather than crashing the server.

## How it works

`PluginLoader` loads each DLL through a `PluginLoadContext` (a custom
`AssemblyLoadContext`) that resolves the shared `Catan` assembly from the host
instead of the plugin folder, so the plugin's types and the host's contract
types have the same identity. It then finds `IGameModePlugin` implementations,
instantiates them, and collects their registrations into the `ModeCatalog`
alongside the built-in modes and the JSON board definitions.
