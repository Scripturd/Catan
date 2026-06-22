# Catan — architecture

A pure .NET game. No Unity, no engine — the whole game is plain C# you can build,
test, and play from the command line. A graphical front-end could be added later as
just another adapter; the game logic would not change.

## Layers — dependencies point inward only

```
Catan.Cli  ──▶  Catan.Application  ──▶  Catan.Domain
(adapters +      (use cases, commands,    (entities, value objects,
 composition       and the ports the        rules — references NOTHING)
 root)             use cases need)
```

- **Catan.Domain** — entities, value objects, and rules. References nothing. The
  "7 activates the robber" rule lives on the `DiceRoll` value object, not in a use case.
- **Catan.Application** — the use cases (one command in, one result out) and the
  **ports** (e.g. `IRandom`) the use cases need from the outside world. References only
  the Domain.
- **Catan.Cli** — the driving adapters (a `System.Random`-backed `IRandom`, the console
  renderer) and the **composition root** that news-up the adapters and injects them into
  the use cases. References Application + Domain.

The rule is enforced by **project references**: `Catan.Domain.csproj` has zero of them,
so it physically cannot depend on the layers above it.

## Use cases & commands

Each player action is a use case: a small class with one `Execute(command)` method,
behind a named interface (`IRollDice`, `IStartTurn`). The *command* is immutable input
data; the use case validates the rules, drives the domain, and returns a result.

Use cases live in vertical slices under `Application/Features/<Feature>/`:

    Features/Dice/
        RollDiceCommand.cs   // input (data)
        IRollDice.cs         // the public contract
        RollDice.cs          // the implementation (leaf use case)

## Use cases may compose other use cases  (the deliberate, "controversial" call)

A slice is **not** an island. An orchestrating use case may call another feature's use
case — `StartTurn` (Turn) calls `IRollDice` (Dice). Strict vertical-slice purism forbids
this; for a game whose rules interlock (build → check victory; roll 7 → robber → discard)
the composition models the real workflow far better than duplication or event spaghetti.

It stays clean because of three rules:

1. **Compose through the public contract, never internals.** A feature depends only on
   another feature's *interface* (`IRollDice`), injected via the constructor — never its
   private handlers or state. This is the line between composition and a ball of mud.
2. **Keep the call graph acyclic.** Orchestrators call leaves (and other orchestrators)
   in one direction only. If you ever want A↔B, the shared logic is really a *domain*
   concept — push it down into `Catan.Domain` instead.
3. **The outermost use case owns the unit of work.** Sub-use-cases operate on the state
   they're handed and return results/events; they don't each independently persist.

Because everything is constructor-injected interfaces, the whole call graph is visible
in the code (no mediator magic) and every orchestrator is testable against a stubbed
sub-use-case (see `StartTurnTests`).

## The litmus test

If logic can't be exercised by a test in `*.Tests` without a console or a screen, it has
leaked out of the core. Pull it back in.
