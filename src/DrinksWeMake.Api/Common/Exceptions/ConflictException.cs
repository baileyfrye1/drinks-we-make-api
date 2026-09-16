namespace DrinksWeMake.Api.Common.Exceptions;

public class ConflictException(string message) : HttpStatusException(message)
{
    public override int StatusCode => StatusCodes.Status409Conflict;
}