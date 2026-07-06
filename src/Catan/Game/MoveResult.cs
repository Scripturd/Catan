namespace Catan.Game;

public sealed record MoveResult
{
    public bool Success { get; }
    public string? Error { get; }

    private MoveResult(bool success, string? error)
    {
        Success = success;
        Error = error;
    }

    public static MoveResult Accepted() => new(true, null);
    public static MoveResult Rejected(string reason) => new(false, reason);
}