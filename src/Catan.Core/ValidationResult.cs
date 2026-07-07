namespace Catan;

public sealed record ValidationResult
{
    public bool Success { get; }
    public string? Reason { get; }

    private ValidationResult(bool success, string? reason)
    {
        Success = success;
        Reason = reason;
    }

    public static ValidationResult Accepted() => new(true, null);
    public static ValidationResult Rejected(string reason) => new(false, reason);
}