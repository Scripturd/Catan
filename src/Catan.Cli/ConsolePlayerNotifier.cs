namespace Catan.Cli;

public sealed class ConsolePlayerNotifier : IPlayerNotifier
{
    public void Info(string message) => Console.WriteLine(message);
    public void Warning(string message) => Console.WriteLine($"Warning: {message}");
    public void Error(string message) => Console.WriteLine($"Error: {message}");
}