namespace DrinksWeMake.Api.Data.Entities;

public interface IUserCocktailEntity
{
    public int CocktailId { get; set; }
		
    public string UserId { get; set; }
}