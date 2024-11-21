namespace Shared.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException()
        : base("Bad request") { }

    public BadRequestException(string message, string details)
        : base(message)
    {
        Details = details;
    }

    public string? Details { get; }
}
