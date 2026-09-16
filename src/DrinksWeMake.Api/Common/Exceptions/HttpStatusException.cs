namespace DrinksWeMake.Api.Common.Exceptions;

public abstract class HttpStatusException(string message) : Exception(message)
{
    public abstract int StatusCode { get; }
}