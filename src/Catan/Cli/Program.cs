using Catan.Game;
using Catan.Players;
using System.Diagnostics;

namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        CompositionRoot compositionRoot = new();

        List<IGameMode> gameModes = [compositionRoot.StandardBoard, compositionRoot.SeafarersScenario1Board];

        Console.WriteLine("Pick a game mode:");
        for (int i = 0; i < gameModes.Count; i++)
            Console.WriteLine($"({i}) {gameModes[i]}");

        IGameMode gameMode = gameModes[UI.AskUserForInt(min: 0, max: 1)];

        var playerCount = UI.AskUserForInt("How many players?", min: gameMode.MinPlayerCount, max: gameMode.MaxPlayerCount);

        List<PlayerId> players = [];
        for (int i = 0; i < playerCount; i++)
        {
            var player = new PlayerId(i);
            players.Add(player);
        }

        gameMode.Start(players);

        var html = HtmlBoardRenderer.ToHtml(compositionRoot.BoardService, compositionRoot.NumberTokenService, compositionRoot.HarbourService, compositionRoot.Robber, compositionRoot.Pirate);
        var path = Path.Combine(Path.GetTempPath(), "catan-board.html");
        File.WriteAllText(path, html);

        Console.WriteLine();
        Console.WriteLine($"Board written to {path} — opening in your browser...");
        OpenInBrowser(path);
    }

    private static void OpenInBrowser(string path)
    {
        try
        {
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }
        catch
        {
            Console.WriteLine("Could not launch a browser automatically; open the file above manually.");
        }
    }
}