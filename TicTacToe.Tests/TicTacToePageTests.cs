using Bunit;
using Microsoft.AspNetCore.Components;
using TicTacToe.Components.Pages;
using TicTacToe.Services;

namespace TicTacToe.Tests;

public class TicTacToePageTests : TestContext
{
    [Fact]
    public void NewGame_RendersNineEmptyCellsAndXStatus()
    {
        var cut = Render<Home>();

        Assert.Equal(9, cut.FindAll("button").Count(button =>
            FindCellIndex(button) >= 0));
        Assert.All(Enumerable.Range(0, 9), cell =>
            Assert.True(string.IsNullOrWhiteSpace(GetCellText(cut, cell))));
        Assert.Contains("Current player: X", cut.Markup);
    }

    [Fact]
    public void HumanMove_PutsXInSelectedCell()
    {
        var cut = Render<Home>();

        ClickCell(cut, 4);

        Assert.Equal("X", GetCellText(cut, 4).Trim());
    }

    [Fact]
    public void ComputerTurn_PutsOneOInAnEmptyCell()
    {
        var cut = Render<Home>();

        ClickCell(cut, 4);

        Assert.Single(cut.FindAll("button").Where(button =>
            FindCellIndex(button) >= 0).Where(button =>
            button.TextContent.Trim() == "O"));
    }

    [Fact]
    public void OccupiedCellClick_DoesNotChangeBoardOrTurn()
    {
        var cut = Render<Home>();
        ClickCell(cut, 4);
        var before = GetBoardSnapshot(cut);
        var statusBefore = GetStatus(cut);

        ClickCell(cut, 4);

        Assert.Equal(before, GetBoardSnapshot(cut));
        Assert.Equal(statusBefore, GetStatus(cut));
    }

    [Fact]
    public void WinnerFinish_ShowsWinnerAndBlocksLaterMoves()
    {
        var cut = Render<Home>();
        FinishAnyGame(cut);

        Assert.True(cut.Markup.Contains("Winner: X")
            || cut.Markup.Contains("Winner: O"));
        var finished = GetBoardSnapshot(cut);

        var emptyCell = FindFirstEmpty(cut);
        if (emptyCell >= 0)
            ClickCell(cut, emptyCell);

        Assert.Equal(finished, GetBoardSnapshot(cut));
    }

    [Fact]
    public void DrawFinish_ShowsDrawAndBlocksLaterMoves()
    {
        var cut = Render<Home>();

        foreach (var cell in new[] { 0, 8, 2, 5, 3 })
        {
            if (cut.Markup.Contains("Draw"))
                break;
            if (GetCellText(cut, cell).Trim().Length == 0)
                ClickCell(cut, cell);
        }

        Assert.Contains("Draw", cut.Markup);
        var finished = GetBoardSnapshot(cut);
        var emptyCell = FindFirstEmpty(cut);
        if (emptyCell >= 0)
            ClickCell(cut, emptyCell);
        Assert.Equal(finished, GetBoardSnapshot(cut));
    }

    [Fact]
    public void XWin_IncreasesHumanScoreExactlyOnce()
    {
        var cut = Render<Home>();
        SetGameBoardNearTerminal(cut.Instance,
        [
            Mark.X, Mark.X, Mark.Empty,
            Mark.O, Mark.O, Mark.Empty,
            Mark.Empty, Mark.Empty, Mark.Empty
        ]);
        cut.Render();

        ClickCell(cut, 2);

        Assert.Contains("Winner: X", cut.Markup);
        AssertScoreLabels(cut, 1, 0, 0);
        ClickFirstAvailableAfterFinish(cut);
        AssertScoreLabels(cut, 1, 0, 0);
    }

    [Fact]
    public void OWin_IncreasesComputerScoreExactlyOnce()
    {
        var cut = Render<Home>();

        foreach (var cell in new[] { 0, 1, 3 })
        {
            if (IsFinished(cut))
                break;
            if (GetCellText(cut, cell).Trim().Length == 0)
                ClickCell(cut, cell);
        }

        Assert.Contains("Winner: O", cut.Markup);
        AssertScoreLabels(cut, 0, 1, 0);
        ClickTerminalMoveAttempt(cut);
        AssertScoreLabels(cut, 0, 1, 0);
    }

    [Fact]
    public void Draw_IncreasesDrawScoreExactlyOnce()
    {
        var cut = Render<Home>();

        foreach (var cell in new[] { 0, 1, 6, 5, 8 })
        {
            if (IsFinished(cut))
                break;
            if (GetCellText(cut, cell).Trim().Length == 0)
                ClickCell(cut, cell);
        }

        Assert.Contains("Draw", cut.Markup);
        AssertScoreLabels(cut, 0, 0, 1);
        ClickTerminalMoveAttempt(cut);
        AssertScoreLabels(cut, 0, 0, 1);
    }

    [Fact]
    public void Rematch_PreservesScoresAndRestoresInitialBoard()
    {
        var cut = Render<Home>();
        SetPrivateField(cut.Instance, "humanWins", 2);
        SetPrivateField(cut.Instance, "computerWins", 3);
        SetPrivateField(cut.Instance, "draws", 4);
        cut.Render();
        FinishAnyGame(cut);
        var humanWins = GetPrivateIntField(cut.Instance, "humanWins");
        var computerWins = GetPrivateIntField(cut.Instance, "computerWins");
        var draws = GetPrivateIntField(cut.Instance, "draws");
        AssertScoreLabels(cut, humanWins, computerWins, draws);

        Click(cut, "Rematch");

        Assert.All(Enumerable.Range(0, 9), cell =>
            Assert.True(string.IsNullOrWhiteSpace(GetCellText(cut, cell))));
        Assert.Contains("Current player: X", cut.Markup);
        AssertScoreLabels(cut, humanWins, computerWins, draws);
    }

    private static void FinishAnyGame(IRenderedComponent<Home> cut)
    {
        for (var pass = 0; pass < 9 && !IsFinished(cut); pass++)
        {
            var empty = FindFirstEmpty(cut);
            if (empty >= 0)
                ClickCell(cut, empty);
        }
    }

    private static bool IsFinished(IRenderedComponent<Home> cut) =>
        cut.Markup.Contains("Winner:")
        || cut.Markup.Contains("Draw");

    private static void ClickCell(IRenderedComponent<Home> cut, int cell)
    {
        var button = cut.FindAll("button").FirstOrDefault(candidate =>
            FindCellIndex(candidate) == cell);
        Assert.NotNull(button);
        button.Click();
    }

    private static void Click(IRenderedComponent<Home> cut, string text)
    {
        var button = cut.FindAll("button")
            .Single(candidate => candidate.TextContent.Trim() == text);
        button.Click();
    }

    private static string GetCellText(IRenderedComponent<Home> cut, int cell)
    {
        var button = cut.FindAll("button").First(candidate =>
            FindCellIndex(candidate) == cell);
        return button.TextContent;
    }

    private static int FindFirstEmpty(IRenderedComponent<Home> cut)
    {
        for (var cell = 0; cell < 9; cell++)
        {
            if (string.IsNullOrWhiteSpace(GetCellText(cut, cell)))
                return cell;
        }
        return -1;
    }

    private static int FindCellIndex(AngleSharp.Dom.IElement button)
    {
        var label = button.GetAttribute("aria-label");
        if (label is not null
            && label.StartsWith("cell-", StringComparison.Ordinal)
            && int.TryParse(label[5..], out var labelledIndex))
            return labelledIndex;

        var id = button.Id;
        if (id.StartsWith("cell-", StringComparison.Ordinal)
            && int.TryParse(id[5..], out var idIndex))
            return idIndex;

        return -1;
    }

    private static string GetBoardSnapshot(IRenderedComponent<Home> cut) =>
        string.Concat(Enumerable.Range(0, 9).Select(cell =>
            GetCellText(cut, cell).Trim() is { Length: > 0 } mark ? mark : "-"));

    private static void AssertScoreLabels(
        IRenderedComponent<Home> cut,
        int humanWins,
        int computerWins,
        int draws)
    {
        Assert.Contains($"Human wins: {humanWins}", cut.Markup);
        Assert.Contains($"Computer wins: {computerWins}", cut.Markup);
        Assert.Contains($"Draws: {draws}", cut.Markup);
    }

    private static void ClickFirstAvailableAfterFinish(IRenderedComponent<Home> cut)
    {
        var emptyCell = FindFirstEmpty(cut);
        if (emptyCell >= 0)
            ClickCell(cut, emptyCell);
    }

    private static void ClickTerminalMoveAttempt(IRenderedComponent<Home> cut)
    {
        ClickCell(cut, 0);
    }

    private static void SetGameBoardNearTerminal(object page, Mark[] board)
    {
        var gameField = page.GetType().GetFields(
                System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic)
            .FirstOrDefault(field =>
                typeof(TicTacToeGame).IsAssignableFrom(field.FieldType));
        Assert.NotNull(gameField);

        var game = Assert.IsType<TicTacToeGame>(gameField.GetValue(page));
        var boardField = typeof(TicTacToeGame).GetField(
            "board",
            System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.NonPublic);
        Assert.NotNull(boardField);

        var engineBoard = Assert.IsType<Mark[]>(boardField.GetValue(game));
        for (var cell = 0; cell < engineBoard.Length; cell++)
            engineBoard[cell] = board[cell];

        SetPrivateField(game, "<CurrentPlayer>k__BackingField", Mark.X);
        SetPrivateField(game, "<Outcome>k__BackingField", GameOutcome.InProgress);
    }

    private static string GetStatus(IRenderedComponent<Home> cut)
    {
        foreach (var status in new[]
                 {
                     "Current player: X",
                     "Current player: O",
                     "Winner: X",
                     "Winner: O",
                     "Draw"
                 })
        {
            if (cut.Markup.Contains(status))
                return status;
        }
        return string.Empty;
    }

    private static void SetPrivateField(object instance, string fieldName, object value)
    {
        var field = instance.GetType().GetField(
            fieldName,
            System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.NonPublic);
        Assert.NotNull(field);
        field.SetValue(instance, value);
    }

    private static int GetPrivateIntField(object instance, string fieldName)
    {
        var field = instance.GetType().GetField(
            fieldName,
            System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.NonPublic);
        Assert.NotNull(field);
        return Assert.IsType<int>(field.GetValue(instance));
    }
}
