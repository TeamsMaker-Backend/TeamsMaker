namespace TeamsMaker.Api.Core.Guards.Exceptions;

public class GuardException : ArgumentException
{
    public GuardException(string message = "Guard: Invalid Data") : base(message)
    { }
}
