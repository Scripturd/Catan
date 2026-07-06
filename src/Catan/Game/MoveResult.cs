namespace Catan.Game;

public sealed record MoveResult
{
    public bool Ok { get; }
    public string? Error { get; }

    private MoveResult(bool ok, string? error)
    {
        Ok = ok;
        Error = error;
    }

    public static MoveResult Accepted() => new(true, null);
    public static MoveResult Rejected(string reason) => new(false, reason);
}
