# Crew Assignments: 001 tic-tac-toe

Total: 13 tasks | engineer: 7 | check-pilot: 6 | captain: 0

Legend: `[ ]` open, `[x]` done, `[P]` can run in parallel

## Phase 1: Setup
- [x] **T001** Scaffold the .NET 10 web host | seat: engineer | risk: low | note: created the five host files without restoring packages
  - Files: `TicTacToe/TicTacToe.csproj`, `TicTacToe/Program.cs`, `TicTacToe/appsettings.json`, `TicTacToe/appsettings.Development.json`, `TicTacToe/Properties/launchSettings.json`
  - Traces to: R1-R7, AC1-AC9
  - Depends on: none
  - Done when: the five-file `dotnet new web` scaffold targets `net10.0`, and `Program.cs` configures the framework Razor Components and Interactive Server host without extra application packages.
- [x] **T002** Add Interactive Server root components | seat: engineer | risk: low | note: added the planned app root, router, and imports
  - Files: `TicTacToe/Components/App.razor`, `TicTacToe/Components/Routes.razor`, `TicTacToe/Components/_Imports.razor`
  - Traces to: R1-R7, AC1-AC9
  - Depends on: T001
  - Done when: the app root uses Interactive Server rendering and routes Razor components at `/` without adding unrelated pages.
- [x] **T003** Verify the host scaffold | seat: check-pilot | risk: - | note: host build passed with zero warnings or errors
  - Files: none (read-only verification)
  - Traces to: R1, AC1
  - Depends on: T002
  - Done when: verdict PASS with `dotnet build TicTacToe/TicTacToe.csproj` output and confirmation that changes match the route.

## Phase 2: Tests first
- [x] **T004** Add engine tests and the approved test project | seat: engineer | risk: medium | note: added cached minimax outcome assertions and a legal final-cell draw test
  - Files: `TicTacToe.Tests/TicTacToe.Tests.csproj`, `TicTacToe.Tests/TicTacToeGameTests.cs`
  - Traces to: R1-R6, AC1-AC7
  - Depends on: T003
  - Done when: the test project references `TicTacToe`, includes only the approved `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio`, and `bunit` packages, and defines engine cases for initial state, legal/invalid moves, all win lines through the approved internal outcome evaluator, draw, terminal moves, and optimal play.
- [x] **T005** Review engine test coverage | seat: check-pilot | risk: - | note: PASS on independent re-review; runtime tests deferred until T006
  - Files: none (read-only review)
  - Traces to: R1-R6, AC1-AC7
  - Depends on: T004
  - Done when: verdict PASS with evidence that the planned engine tests cover the route contracts and do not request unapproved dependencies; implementation-dependent tests are not run yet.

## Phase 3: Core
- [x] **T006** Implement the optimal game engine | seat: engineer | risk: high | note: implemented the public engine contract and minimax; 31 tests passed
  - Files: `TicTacToe/Services/TicTacToeGame.cs`
  - Traces to: R1-R6, AC1-AC7
  - Depends on: T005
  - Done when: the public engine contract and approved internal outcome evaluator in the route are implemented, and `dotnet test TicTacToe.Tests/TicTacToe.Tests.csproj` passes, including every win-line case and exhaustive optimal-play coverage.
- [x] **T007** Verify engine rules and optimal play | seat: check-pilot | risk: - | note: PASS; 31 tests passed and win/draw/invalid/terminal/minimax behavior independently reviewed
  - Files: none (read-only verification)
  - Traces to: R1-R6, AC1-AC7
  - Depends on: T006
  - Done when: verdict PASS with the exact passing test command/output and independent review of win, draw, invalid-move, terminal-state, and minimax behavior.

## Phase 4: Integration
- [ ] **T008** Add component acceptance tests | seat: engineer | risk: medium | note: HOLD after re-review; fix AC9 rematch score capture and consistently exercise terminal-move score stability for O-win/draw
- [x] **T008** Add component acceptance tests | seat: engineer | risk: medium | note: added AC8/AC9 coverage and migrated bUnit rendering calls to the supported Render<Home>() API
  - Files: `TicTacToe.Tests/TicTacToePageTests.cs`
  - Traces to: R1-R7, AC1-AC3, AC5-AC9
  - Depends on: T007
  - Done when: component tests specify initial board/status, human/computer moves, occupied-cell no-op, win/draw outcomes, score increments, and rematch score retention.
- [x] **T009** Review component acceptance tests | seat: check-pilot | risk: - | note: PASS for coverage and bUnit migration; runtime component execution remains blocked by Windows Application Control
  - Files: none (read-only review)
  - Traces to: R1-R7, AC1-AC3, AC5-AC9
  - Depends on: T008
  - Done when: verdict PASS with evidence that the test scenarios map to the flight plan; implementation-dependent tests are not run yet.
- [ ] **T010** Build the game page and session scoring | seat: engineer | risk: high | note: UI/build complete; suite blocked by AngleSharp.dll Application Control policy
  - Files: `TicTacToe/Components/Pages/Home.razor`, `TicTacToe/Components/Pages/Home.razor.css`
  - Traces to: R1-R7, AC1-AC9
  - Depends on: T009
  - Done when: `/` renders the 3-by-3 game, calls the engine for human/computer turns, blocks invalid or terminal moves, records exactly one outcome score, and rematches reset the board while preserving scores; full test suite passes.
- [ ] **T011** Verify game interactions and scores | seat: check-pilot | risk: -
  - Files: none (read-only verification)
  - Traces to: R1-R7, AC1-AC9
  - Depends on: T010
  - Done when: verdict PASS with `dotnet build TicTacToe/TicTacToe.csproj`, `dotnet test TicTacToe.Tests/TicTacToe.Tests.csproj`, and manual browser evidence for initial render, turn flow, completion, invalid move, scoring, and rematch.

## Phase 5: Polish
- [ ] **T012** Document .NET quality gates | seat: engineer | risk: low
  - Files: `.flightcrew/manual.md`
  - Traces to: R1-R7, AC1-AC9 (quality gates)
  - Depends on: T011
  - Done when: Tech standards document C#/.NET 10 and Blazor Interactive Server, and Quality gates list the exact build, test, whitespace, and `dotnet run`/browser verification steps.
- [ ] **T013** Verify final evidence and scope | seat: check-pilot | risk: -
  - Files: none (read-only verification)
  - Traces to: R1-R7, AC1-AC9
  - Depends on: T012
  - Done when: verdict PASS with passing final quality-gate evidence, acceptance-check coverage, and confirmation that changed files are limited to the approved route.
