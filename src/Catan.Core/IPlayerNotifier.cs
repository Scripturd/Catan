namespace Catan;

public interface IPlayerNotifier
{
    void Info(string message);
    void Warning(string message);
    void Error(string message);
}