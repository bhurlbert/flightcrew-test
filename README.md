# Flightcrew Test: Tic-Tac-Toe Demo

This repository demonstrates the Flightcrew workflow by taking a software change from an initial repository survey through planning, implementation, and independent verification. The result is a browser-based tic-tac-toe game: a human plays X against an optimal computer opponent playing O.

## Run the demo

Requires the .NET 10 SDK.

```powershell
dotnet run --project TicTacToe/TicTacToe.csproj
```

Open the local address printed by `dotnet run` (normally <http://localhost:5225/>). Select an empty cell to play X; the computer responds with O. The computer cannot be beaten. After a game ends, select **Rematch** to clear the board. Human wins, computer wins, and draws are counted for the current browser session.

## What the app includes

- A responsive, Flightcrew-branded Blazor Interactive Server interface.
- A 3-by-3 board, turn and outcome status, rematches, and session scores.
- A separate game engine using minimax for optimal computer play.
- Engine and component acceptance tests in `TicTacToe.Tests/`.

## How Flightcrew created the demo

Flight 001, `001-tic-tac-toe`, followed the repository's documented crew workflow:

1. **Survey** — recorded the repository baseline and existing risks in `.flightcrew/survey/baseline.md`.
2. **Flight plan and briefing** — established the user scenarios, requirements, acceptance checks, and decisions: browser-based play, human X versus computer O, optimal play, and session-only scores.
3. **Route** — chose a minimal .NET 10 ASP.NET Core host with Blazor Interactive Server, a separately testable game engine, and component acceptance tests.
4. **Preflight** — checked requirement-to-acceptance-check traceability, scope, task dependencies, approvals, and risk controls before implementation.
5. **Assignments and flight** — split the work into ordered Engineer and independent Check Pilot tasks. The engine tests were written before the engine; Check Pilot reviews identified missing score-transition cases, which were corrected before continuing.
6. **Integration and polish** — added the game UI and session scoring, linked the CSS-isolation bundle, and applied the Flightcrew visual identity.

The flight plan, route, preflight report, assignments, and append-only execution history are in [`.flightcrew/flights/001-tic-tac-toe/`](.flightcrew/flights/001-tic-tac-toe/). The project-wide process and guardrails are described in [`.flightcrew/manual.md`](.flightcrew/manual.md).

## Validation

Build the app:

```powershell
dotnet build TicTacToe/TicTacToe.csproj
```

Run the automated tests:

```powershell
dotnet test TicTacToe.Tests/TicTacToe.Tests.csproj
```

The game engine's 31 tests have passed, and the app has built and been manually exercised in a browser, including a human move and computer response. In this environment, the component tests have been blocked at runtime because Windows Application Control prevents loading the transitive `AngleSharp.dll` (`0x800711C7`); this is an environment policy failure, not a component assertion result. See the flight assignments and black box for the latest recorded verification state.
