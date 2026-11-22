global using Xunit;

using TicTacToe.Services;

namespace TicTacToe.Tests;

public class TicTacToeGameTests
{
    [Fact]
    public void NewGame_HasEmptyBoard_XTurnAndInProgressOutcome()
    {
        var game = new TicTacToeGame();

        Assert.Equal(9, game.Board.Count);
        Assert.All(game.Board, mark => Assert.Equal(Mark.Empty, mark));
        Assert.Equal(Mark.X, game.CurrentPlayer);
        Assert.Equal(GameOutcome.InProgress, game.Outcome);
    }

    [Fact]
    public void Reset_RestoresInitialState()
    {
        var game = new TicTacToeGame();

        Assert.True(game.TryHumanMove(4));
        game.Reset();

        Assert.Equal(9, game.Board.Count);
        Assert.All(game.Board, mark => Assert.Equal(Mark.Empty, mark));
        Assert.Equal(Mark.X, game.CurrentPlayer);
        Assert.Equal(GameOutcome.InProgress, game.Outcome);
    }

    [Fact]
    public void TryHumanMove_AppliesLegalMoveAndSetsCurrentPlayerToO()
    {
        var game = new TicTacToeGame();

        Assert.True(game.TryHumanMove(4));
        Assert.Equal(Mark.X, game.Board[4]);
        Assert.Equal(Mark.O, game.CurrentPlayer);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(9)]
    [InlineData(100)]
    public void TryHumanMove_RejectsOutOfRangeCells(int cell)
    {
        var game = new TicTacToeGame();
        var before = game.Board.ToArray();

        Assert.False(game.TryHumanMove(cell));
        Assert.Equal(before, game.Board);
        Assert.Equal(Mark.X, game.CurrentPlayer);
        Assert.Equal(GameOutcome.InProgress, game.Outcome);
    }

    [Fact]
    public void TryHumanMove_RejectsOccupiedCellWithoutChangingState()
    {
        var game = new TicTacToeGame();
        Assert.True(game.TryHumanMove(4));
        var before = game.Board.ToArray();

        Assert.False(game.TryHumanMove(4));
        Assert.Equal(before, game.Board);
        Assert.Equal(Mark.O, game.CurrentPlayer);
    }

    [Fact]
    public void TryHumanMove_DoesNothingWhenOIsCurrentPlayer()
    {
        var game = new TicTacToeGame();
        Assert.True(game.TryHumanMove(0));
        var before = game.Board.ToArray();

        Assert.False(game.TryHumanMove(1));
        Assert.Equal(before, game.Board);
        Assert.Equal(Mark.O, game.CurrentPlayer);
    }

    [Fact]
    public void TryComputerMove_AddsOneLegalOMoveAndReturnsTurnToX()
    {
        var game = new TicTacToeGame();
        Assert.True(game.TryHumanMove(4));
        var before = game.Board.Count(mark => mark != Mark.Empty);

        Assert.True(game.TryComputerMove());
        Assert.Equal(before + 1, game.Board.Count(mark => mark != Mark.Empty));
        Assert.Single(game.Board.Where(mark => mark == Mark.O));
        Assert.Equal(Mark.X, game.CurrentPlayer);
    }

    [Fact]
    public void TryComputerMove_ReturnsFalseBeforeHumanMove()
    {
        var game = new TicTacToeGame();

        Assert.False(game.TryComputerMove());
        Assert.All(game.Board, mark => Assert.Equal(Mark.Empty, mark));
        Assert.Equal(Mark.X, game.CurrentPlayer);
    }

    public static IEnumerable<object[]> WinningLines()
    {
        int[][] lines =
        [
            [0, 1, 2],
            [3, 4, 5],
            [6, 7, 8],
            [0, 3, 6],
            [1, 4, 7],
            [2, 5, 8],
            [0, 4, 8],
            [2, 4, 6]
        ];

        foreach (var mark in new[] { Mark.X, Mark.O })
        {
            foreach (var line in lines)
                yield return [mark, line];
        }
    }

    [Theory]
    [MemberData(nameof(WinningLines))]
    public void EvaluateOutcome_DetectsEveryWinLineForXAndO(Mark mark, int[] line)
    {
        var board = Enumerable.Repeat(Mark.Empty, 9).ToArray();
        foreach (var cell in line)
            board[cell] = mark;

        var expected = mark == Mark.X ? GameOutcome.XWins : GameOutcome.OWins;
        Assert.Equal(expected, TicTacToeGame.EvaluateOutcome(board));
    }

    [Fact]
    public void EvaluateOutcome_DetectsDraw()
    {
        IReadOnlyList<Mark> board =
        [
            Mark.X, Mark.O, Mark.X,
            Mark.X, Mark.O, Mark.O,
            Mark.O, Mark.X, Mark.X
        ];

        Assert.Equal(GameOutcome.Draw, TicTacToeGame.EvaluateOutcome(board));
    }

    [Fact]
    public void EvaluateOutcome_KeepsIncompleteBoardInProgress()
    {
        IReadOnlyList<Mark> board =
        [
            Mark.X, Mark.Empty, Mark.O,
            Mark.Empty, Mark.Empty, Mark.Empty,
            Mark.Empty, Mark.Empty, Mark.Empty
        ];

        Assert.Equal(GameOutcome.InProgress, TicTacToeGame.EvaluateOutcome(board));
    }

    [Fact]
    public void TerminalGame_RejectsHumanAndComputerMoves()
    {
        var terminalSequence = FindTerminalSequence();
        Assert.NotNull(terminalSequence);
        var game = Replay(gameSequence: terminalSequence);

        var board = game.Board.ToArray();
        Assert.NotEqual(GameOutcome.InProgress, game.Outcome);

        Assert.False(game.TryHumanMove(FindFirstEmpty(board)));
        Assert.False(game.TryComputerMove());
        Assert.Equal(board, game.Board);
    }

    [Fact]
    public void OptimalComputerPlay_PreservesBestOutcomeAndTakesForcedWinsAcrossReachableSequences()
    {
        var game = new TicTacToeGame();
        var humanSequence = new List<int>();
        var referenceOutcomes = new Dictionary<string, GameOutcome>();

        var foundForcedWin = ExploreHumanSequences(game, humanSequence, referenceOutcomes);

        Assert.True(foundForcedWin, "The exhaustive traversal should encounter a reachable O-turn where O can force a win.");
    }

    [Fact]
    public void LegalGameplay_FillsFinalCellAsDrawAndRejectsFurtherMoves()
    {
        var humanSequence = FindDrawSequence();
        Assert.NotNull(humanSequence);

        var game = Replay(humanSequence.Take(humanSequence.Count - 1).ToList());
        Assert.Equal(GameOutcome.InProgress, game.Outcome);
        Assert.Equal(Mark.X, game.CurrentPlayer);
        Assert.Single(game.Board.Where(mark => mark == Mark.Empty));
        Assert.Equal(GameOutcome.InProgress, TicTacToeGame.EvaluateOutcome(game.Board));

        var finalCell = humanSequence[^1];
        Assert.Equal(finalCell, FindFirstEmpty(game.Board.ToArray()));
        Assert.True(game.TryHumanMove(finalCell));

        Assert.Equal(GameOutcome.Draw, game.Outcome);
        Assert.DoesNotContain(Mark.Empty, game.Board);
        Assert.Equal(GameOutcome.Draw, TicTacToeGame.EvaluateOutcome(game.Board));

        var board = game.Board.ToArray();
        Assert.False(game.TryHumanMove(finalCell));
        Assert.False(game.TryComputerMove());
        Assert.Equal(board, game.Board);
    }

    private static bool ExploreHumanSequences(
        TicTacToeGame game,
        List<int> humanSequence,
        Dictionary<string, GameOutcome> referenceOutcomes)
    {
        var foundForcedWin = false;

        if (game.Outcome != GameOutcome.InProgress)
        {
            Assert.NotEqual(GameOutcome.XWins, game.Outcome);
            return false;
        }

        for (var cell = 0; cell < 9; cell++)
        {
            if (game.Board[cell] != Mark.Empty)
                continue;

            var nextHumanSequence = humanSequence.Append(cell).ToList();
            var nextGame = ReplayBeforeComputer(nextHumanSequence);

            if (nextGame.Outcome != GameOutcome.InProgress)
            {
                Assert.NotEqual(GameOutcome.XWins, nextGame.Outcome);
                continue;
            }

            Assert.Equal(Mark.O, nextGame.CurrentPlayer);
            var bestOutcome = BestAchievableOutcome(nextGame.Board, Mark.O, referenceOutcomes);
            Assert.True(nextGame.TryComputerMove());

            var achievedOutcome = BestAchievableOutcome(nextGame.Board, nextGame.CurrentPlayer, referenceOutcomes);
            Assert.Equal(bestOutcome, achievedOutcome);
            if (bestOutcome == GameOutcome.OWins)
                foundForcedWin = true;

            if (nextGame.Outcome == GameOutcome.InProgress)
                foundForcedWin |= ExploreHumanSequences(nextGame, nextHumanSequence, referenceOutcomes);
        }

        return foundForcedWin;
    }

    private static GameOutcome BestAchievableOutcome(
        IReadOnlyList<Mark> board,
        Mark currentPlayer,
        Dictionary<string, GameOutcome> referenceOutcomes)
    {
        return SearchBestOutcome(board.ToArray(), currentPlayer, referenceOutcomes);
    }

    private static GameOutcome SearchBestOutcome(
        Mark[] board,
        Mark currentPlayer,
        Dictionary<string, GameOutcome> referenceOutcomes)
    {
        var outcome = TicTacToeGame.EvaluateOutcome(board);
        if (outcome != GameOutcome.InProgress)
            return outcome;

        var key = new string(board.Select(mark => (char)('0' + (int)mark)).ToArray())
            + (char)('0' + (int)currentPlayer);
        if (referenceOutcomes.TryGetValue(key, out var cachedOutcome))
            return cachedOutcome;

        var bestOutcome = currentPlayer == Mark.O ? GameOutcome.XWins : GameOutcome.OWins;
        for (var cell = 0; cell < board.Length; cell++)
        {
            if (board[cell] != Mark.Empty)
                continue;

            board[cell] = currentPlayer;
            var candidate = SearchBestOutcome(
                board,
                currentPlayer == Mark.O ? Mark.X : Mark.O,
                referenceOutcomes);
            board[cell] = Mark.Empty;

            if (currentPlayer == Mark.O
                ? OutcomeRank(candidate) > OutcomeRank(bestOutcome)
                : OutcomeRank(candidate) < OutcomeRank(bestOutcome))
            {
                bestOutcome = candidate;
            }
        }

        referenceOutcomes.Add(key, bestOutcome);
        return bestOutcome;
    }

    private static int OutcomeRank(GameOutcome outcome)
    {
        return outcome switch
        {
            GameOutcome.XWins => 0,
            GameOutcome.Draw => 1,
            GameOutcome.OWins => 2,
            _ => throw new InvalidOperationException("Only terminal outcomes have a rank.")
        };
    }

    private static List<int>? FindDrawSequence()
    {
        return FindDrawSequence(new List<int>());
    }

    private static List<int>? FindDrawSequence(List<int> humanSequence)
    {
        var game = Replay(humanSequence);
        if (game.Outcome == GameOutcome.Draw)
            return humanSequence;
        if (game.Outcome != GameOutcome.InProgress)
            return null;

        for (var cell = 0; cell < 9; cell++)
        {
            if (game.Board[cell] != Mark.Empty)
                continue;

            var nextHumanSequence = humanSequence.Append(cell).ToList();
            var drawSequence = FindDrawSequence(nextHumanSequence);
            if (drawSequence is not null)
                return drawSequence;
        }

        return null;
    }

    private static TicTacToeGame Replay(List<int>? gameSequence)
    {
        var game = new TicTacToeGame();
        if (gameSequence is null)
            return game;

        foreach (var cell in gameSequence)
        {
            if (game.Outcome != GameOutcome.InProgress)
                break;
            Assert.True(game.TryHumanMove(cell));
            if (game.Outcome == GameOutcome.InProgress)
                Assert.True(game.TryComputerMove());
        }
        return game;
    }

    private static TicTacToeGame ReplayBeforeComputer(List<int> humanSequence)
    {
        var game = new TicTacToeGame();
        for (var index = 0; index < humanSequence.Count; index++)
        {
            if (game.Outcome != GameOutcome.InProgress)
                break;
            Assert.True(game.TryHumanMove(humanSequence[index]));
            if (index < humanSequence.Count - 1 && game.Outcome == GameOutcome.InProgress)
                Assert.True(game.TryComputerMove());
        }
        return game;
    }

    private static List<int>? FindTerminalSequence()
    {
        for (var first = 0; first < 9; first++)
        {
            var sequence = new List<int> { first };
            var terminal = FindTerminalSequence(sequence);
            if (terminal is not null)
                return terminal;
        }
        return null;
    }

    private static List<int>? FindTerminalSequence(List<int> humanSequence)
    {
        var game = Replay(humanSequence);
        if (game.Outcome != GameOutcome.InProgress)
            return humanSequence;

        for (var cell = 0; cell < 9; cell++)
        {
            if (game.Board[cell] != Mark.Empty)
                continue;

            var next = humanSequence.Append(cell).ToList();
            var terminal = FindTerminalSequence(next);
            if (terminal is not null)
                return terminal;
        }
        return null;
    }

    private static int FindFirstEmpty(Mark[] board)
    {
        for (var cell = 0; cell < board.Length; cell++)
        {
            if (board[cell] == Mark.Empty)
                return cell;
        }
        return 0;
    }
}
