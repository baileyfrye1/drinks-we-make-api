namespace DrinksWeMake.Api.Common.Exceptions;

public class BadRequestException(string message) : HttpStatusException(message)
{
    public override int StatusCode => StatusCodes.Status400BadRequest;
}