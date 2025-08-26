namespace Exceptions;

public class PlaceIsBusyException : Exception
{
    public PlaceIsBusyException(string message) : base(message) {}
}