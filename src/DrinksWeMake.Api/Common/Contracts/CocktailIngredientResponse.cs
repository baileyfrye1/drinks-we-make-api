namespace DrinksWeMake.Api.Common.Contracts;

public sealed record CocktailIngredientResponse(IngredientResponse Ingredient, double Amount, string Unit);