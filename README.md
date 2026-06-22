# Catan

A game of Catan as a pure .NET solution — no game engine. Plain C# you can build, test,
and run from the command line.

## Structure

    src/Catan.Domain          rules & types (references nothing)
    src/Catan.Cli             console runner
    tests/Catan.Domain.Tests  headless tests

## Run

    dotnet run --project src/Catan.Cli

## Test

    dotnet test
