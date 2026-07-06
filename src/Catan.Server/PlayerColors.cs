namespace Catan.Server;

public static class PlayerColors
{
    private static readonly string[] Palette =
    [
        "#d64545",
        "#4a72d6",
        "#e0862a",
        "#3aa657",
        "#9b59b6",
        "#16a085"
    ];

    public static string For(int playerId) => Palette[playerId % Palette.Length];
}
