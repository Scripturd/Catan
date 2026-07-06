namespace Catan.Modding;

public sealed class BoardDefinitionException : Exception
{
    public BoardDefinitionException(string message) : base(message)
    {
    }

    public BoardDefinitionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
