# Flight Plan: 001 tic-tac-toe

Captain: pending | Opened: 2026-11-21 | Status: briefed

## Intent
Build a browser-based tic-tac-toe game in this repository for one human playing against the computer, with rematches and session score tracking.

## Users and scenarios
1. A human opens the game in a browser, sees the board and whose turn it is, and makes moves against the computer.
2. The human and computer continue taking turns until one wins or the board is full with no winner.
3. After a game ends, the human sees the outcome and session scores and can start a rematch.

## In scope
- A browser game in this repository.
- A standard 3-by-3 board with the human playing X and the computer playing O.
- Alternating turns; both the human and computer can occupy only an empty cell.
- An optimal computer opponent that wins when it can force a win and otherwise guarantees at least a draw.
- Win detection for a complete row, column, or diagonal.
- Draw detection when the board is full and no player has won.
- Preventing moves from changing a finished game.
- Rematches that reset the board and retain session scores for human wins, computer wins, and draws.

## Out of scope
- Online play and scores persisted between browser sessions.
- Nonstandard board sizes or win rules.
- Visual design details.

## Requirements
- **R1**: A new game presents nine empty cells arranged as a 3-by-3 board and indicates the current player; X moves first.
- **R2**: On the human's turn, selecting an empty cell places X in that cell.
- **R3**: On the computer's turn, the computer places O in an empty cell and plays optimally: it wins when it can force a win and otherwise guarantees at least a draw.
- **R4**: Selecting an occupied cell does not change the board or current player.
- **R5**: Completing a row, column, or diagonal with one player's marks identifies that player as the winner and ends the game.
- **R6**: Filling all nine cells without a winning line identifies the game as a draw and ends the game.
- **R7**: The game tracks human wins, computer wins, and draws for the current browser session; a rematch clears the board while preserving those scores.

## Acceptance checks
- **AC1** (R1): Given a new game, when its board is presented, then it shows a 3-by-3 grid of empty cells and indicates X as the current player.
- **AC2** (R2): Given it is the human's turn and an empty cell is available, when the human selects that cell, then X appears in that cell.
- **AC3** (R3): Given it is the computer's turn and an empty cell is available, when the computer makes its move, then O appears in one empty cell.
- **AC4** (R3): Given any legal sequence of human moves, when the computer responds optimally throughout the game, then the human never wins and the computer wins whenever it can force a win.
- **AC5** (R4): Given it is the human's turn and a cell is occupied, when the human selects that cell, then the board and current player remain unchanged.
- **AC6** (R5): Given a game in progress, when either player completes any row, column, or diagonal with their marks, then that player is identified as the winner and further moves do not change the game.
- **AC7** (R6): Given a game in progress with no winning line and one empty cell where the next mark will not complete a winning line, when that cell is filled, then the game is identified as a draw and further moves do not change the game.
- **AC8** (R7): Given a game that ends in a win or draw, when the outcome is recorded, then only its matching session score increases by one.
- **AC9** (R7): Given a completed game with session scores, when the human starts a rematch, then the board is empty, X is next, and all session scores are unchanged.

## Impact on existing system
- The baseline describes the repository as a Markdown-only operating framework with no compiled application or test setup (Summary, Tests: [`.flightcrew/survey/baseline.md`]).
- This flight adds its flight plan and flight-local black box under `.flightcrew/flights/001-tic-tac-toe/`.
- The game will add browser-game files to this repository; their exact paths and implementation approach belong in the route because no existing application files are available to extend (Entry points, Hazards: [`.flightcrew/survey/baseline.md`]).
- The ops manual requires captain approval for package installations; the route must not install packages without that approval ([`.flightcrew/manual.md`, Guardrails]).

## Open questions
- None; all questions were resolved during briefing.

## Briefing log
| Date | Question | Decision |
|---|---|---|
| 2026-11-21 | Where should the game run and live? | Browser game in this repository. |
| 2026-11-21 | Who plays? | One human against the computer. |
| 2026-11-21 | Should the game support rematches and scores? | Allow rematches and track scores for the session. |
| 2026-11-21 | How strong should the computer play? | Optimal play: never loses; wins when possible, otherwise draws. |
| 2026-11-21 | Low-impact defaults for mark assignment and session score behavior | Human plays X and moves first; computer plays O. Count human wins, computer wins, and draws; rematches reset the board but retain scores, which are not persisted between browser sessions. Captain may veto these defaults. |
