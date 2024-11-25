namespace Shared.Exceptions;

public class InternalServerException : Exception
{
    public InternalServerException()
        : base("Internal server error") { }

    public InternalServerException(string message, string details)
        : base(message)
    {
        Details = details;
    }

    public string? Details { get; }
}