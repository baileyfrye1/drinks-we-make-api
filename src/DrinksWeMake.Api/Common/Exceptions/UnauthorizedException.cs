namespace DrinksWeMake.Api.Common.Exceptions;

public class UnauthorizedException(string message) : HttpStatusException(message)
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;
}