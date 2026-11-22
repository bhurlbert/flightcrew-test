using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TicTacToe.Tests")]

namespace TicTacToe.Services;

public enum Mark
{
    Empty,
    X,
    O
}

public enum GameOutcome
{
    InProgress,
    XWins,
    OWins,
    Draw
}

public class TicTacToeGame
{
    private readonly Mark[] board;

    public TicTacToeGame()
    {
        board = new Mark[9];
        Board = new ReadOnlyCollection<Mark>(board);
        Reset();
    }

    public IReadOnlyList<Mark> Board { get; }

    public Mark CurrentPlayer { get; private set; }

    public GameOutcome Outcome { get; private set; }

    public bool TryHumanMove(int cell)
    {
        if (Outcome != GameOutcome.InProgress
            || CurrentPlayer != Mark.X
            || cell < 0
            || cell >= board.Length
            || board[cell] != Mark.Empty)
        {
            return false;
        }

        board[cell] = Mark.X;
        Outcome = EvaluateOutcome(Board);
        if (Outcome == GameOutcome.InProgress)
        {
            CurrentPlayer = Mark.O;
        }

        return true;
    }

    public bool TryComputerMove()
    {
        if (Outcome != GameOutcome.InProgress
            || CurrentPlayer != Mark.O)
        {
            return false;
        }

        var bestCell = -1;
        var bestScore = int.MinValue;

        for (var cell = 0; cell < board.Length; cell++)
        {
            if (board[cell] != Mark.Empty)
            {
                continue;
            }

            board[cell] = Mark.O;
            var score = Minimax(board, Mark.X, 1);
            board[cell] = Mark.Empty;

            if (score > bestScore)
            {
                bestScore = score;
                bestCell = cell;
            }
        }

        if (bestCell < 0)
        {
            return false;
        }

        board[bestCell] = Mark.O;
        Outcome = EvaluateOutcome(Board);
        if (Outcome == GameOutcome.InProgress)
        {
            CurrentPlayer = Mark.X;
        }

        return true;
    }

    public void Reset()
    {
        for (var cell = 0; cell < board.Length; cell++)
        {
            board[cell] = Mark.Empty;
        }

        CurrentPlayer = Mark.X;
        Outcome = GameOutcome.InProgress;
    }

    internal static GameOutcome EvaluateOutcome(IReadOnlyList<Mark> board)
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

        foreach (var line in lines)
        {
            var mark = board[line[0]];
            if (mark != Mark.Empty
                && mark == board[line[1]]
                && mark == board[line[2]])
            {
                return mark == Mark.X ? GameOutcome.XWins : GameOutcome.OWins;
            }
        }

        foreach (var mark in board)
        {
            if (mark == Mark.Empty)
            {
                return GameOutcome.InProgress;
            }
        }

        return GameOutcome.Draw;
    }

    private static int Minimax(Mark[] board, Mark currentPlayer, int depth)
    {
        var outcome = EvaluateOutcome(board);
        if (outcome == GameOutcome.OWins)
        {
            return 10 - depth;
        }
        if (outcome == GameOutcome.XWins)
        {
            return depth - 10;
        }
        if (outcome == GameOutcome.Draw)
        {
            return 0;
        }

        if (currentPlayer == Mark.O)
        {
            var bestScore = int.MinValue;
            for (var cell = 0; cell < board.Length; cell++)
            {
                if (board[cell] != Mark.Empty)
                {
                    continue;
                }

                board[cell] = Mark.O;
                var score = Minimax(board, Mark.X, depth + 1);
                board[cell] = Mark.Empty;

                if (score > bestScore)
                {
                    bestScore = score;
                }
            }

            return bestScore;
        }

        var lowestScore = int.MaxValue;
        for (var cell = 0; cell < board.Length; cell++)
        {
            if (board[cell] != Mark.Empty)
            {
                continue;
            }

            board[cell] = Mark.X;
            var score = Minimax(board, Mark.O, depth + 1);
            board[cell] = Mark.Empty;

            if (score < lowestScore)
            {
                lowestScore = score;
            }
        }

        return lowestScore;
    }
}
