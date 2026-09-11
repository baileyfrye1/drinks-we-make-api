namespace DrinksWeMake.Api.Extensions;

public static class StringExtensions
{
    public static string Clean(this string input)
    {
        return input.Trim().ToLowerInvariant();
    }
}