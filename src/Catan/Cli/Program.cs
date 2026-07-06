using Catan.Game;
using Catan.GameModes;
using Catan.Players;
using System.Diagnostics;

namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        CompositionRoot compositionRoot = new();

        var catalog = new ModeCatalog(
            Path.Combine(AppContext.BaseDirectory, "modes"),
            Path.Combine(AppContext.BaseDirectory, "plugins"),
            Console.WriteLine);

        if (catalog.Modes.Count == 0)
        {
            Console.WriteLine("No game modes found. Build the mode plugins into the 'plugins' folder (see PLUGINS.md).");
            return;
        }

        Console.WriteLine("Pick a game mode:");
        for (int i = 0; i < catalog.Modes.Count; i++)
            Console.WriteLine($"({i}) {catalog.Modes[i].Name}");

        IGameMode gameMode = catalog.Modes[UI.AskUserForInt(min: 0, max: catalog.Modes.Count - 1)];

        var playerCount = UI.AskUserForInt("How many players?", min: gameMode.MinPlayerCount, max: gameMode.MaxPlayerCount);

        List<PlayerId> players = [];
        for (int i = 0; i < playerCount; i++)
        {
            var player = new PlayerId(i);
            players.Add(player);
        }

        var services = new GameServices(
            compositionRoot.BoardService,
            compositionRoot.NumberTokenService,
            compositionRoot.HarbourService,
            compositionRoot.Robber,
            compositionRoot.Markers,
            compositionRoot.Shuffler);
        gameMode.Start(services, players);

        var html = HtmlBoardRenderer.ToHtml(compositionRoot.BoardService, compositionRoot.NumberTokenService, compositionRoot.HarbourService, compositionRoot.Robber, compositionRoot.Markers);
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