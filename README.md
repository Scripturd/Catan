# Catan

A game of Catan implemented as a **pure .NET solution** — no game engine. The entire
game (rules, board, turns, scoring) is plain C# you can build, test, and play from the
command line.

The architecture is hexagonal (ports & adapters) with vertical slices and a use-case
driven application layer. See [ARCHITECTURE.md](ARCHITECTURE.md).

## Run it

```bash
dotnet run --project src/Catan.Cli
```

## Test it

```bash
dotnet test
```

A full game can be exercised in the test suite, headless, in milliseconds — that's the
point of keeping the domain and use cases free of any UI or engine.
