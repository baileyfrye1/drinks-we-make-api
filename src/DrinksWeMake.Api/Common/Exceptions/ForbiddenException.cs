namespace DrinksWeMake.Api.Common.Exceptions;

public class ForbiddenException(string message) : HttpStatusException(message)
{
    public override int StatusCode => StatusCodes.Status403Forbidden;
}