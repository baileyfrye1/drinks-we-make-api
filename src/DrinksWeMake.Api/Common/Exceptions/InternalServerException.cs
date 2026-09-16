namespace DrinksWeMake.Api.Common.Exceptions;

public class InternalServerException(string message) : HttpStatusException(message)
{
    public override int StatusCode => StatusCodes.Status500InternalServerError; 
}