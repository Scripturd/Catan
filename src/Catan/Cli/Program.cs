using System.Diagnostics;

namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("Pick an expansion pack:");
        Console.WriteLine("(0) Base Catan");
        Console.WriteLine("(1) Seafarers");
        var expansion = UI.AskUserForInt(min: 0, max: 1);

        var playerCount = UI.AskUserForInt("How many players?", min: 3, max: 4);

        CompositionRoot compositionRoot = new();

        BoardService grid;
        NumberTokenService numberTokens;
        if (expansion == 0)
            (grid, numberTokens) = compositionRoot.StandardBoardGenerator.Create();
        else
            (grid, numberTokens) = playerCount == 3
                ? compositionRoot.SeafarersScenario1ThreePlayer.Create()
                : compositionRoot.SeafarersScenario1FourPlayer.Create();

        var html = HtmlBoardRenderer.ToHtml(grid, numberTokens);
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