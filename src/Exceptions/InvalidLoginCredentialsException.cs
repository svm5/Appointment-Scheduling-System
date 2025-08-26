namespace Exceptions;

public class InvalidLoginCredentialsException : Exception
{
    public InvalidLoginCredentialsException(string message) : base(message) {}   
}