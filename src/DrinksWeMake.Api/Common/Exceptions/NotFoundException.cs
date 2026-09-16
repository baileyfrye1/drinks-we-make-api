namespace DrinksWeMake.Api.Common.Exceptions;

public class NotFoundException(string message) : HttpStatusException(message)
{
    public override int StatusCode => StatusCodes.Status404NotFound;
}