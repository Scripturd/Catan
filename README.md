# Catan

A game of Catan as a pure .NET solution — no game engine. Plain C# you can build, test,
and run from the command line or as a web server.

## Structure

    src/Catan.Core            core rules & types (library)
    src/Catan.Cli             console runner
    src/Catan.Server          ASP.NET Core + SignalR multiplayer server
    src/Catan.Standard        built-in game modes (each its own project,
    src/Catan.Seafarers         referenced by the hosts — see GAME-MODES.md)
    src/Catan.Mini
    tests/Catan.Tests         headless tests

## Build

    dotnet build Catan.slnx

## Run

    dotnet run --project src/Catan.Cli       # console
    dotnet run --project src/Catan.Server     # web server (see HOSTING.md / DEPLOY.md)

## Test

    dotnet test

## Adding a game mode

See [GAME-MODES.md](GAME-MODES.md).
