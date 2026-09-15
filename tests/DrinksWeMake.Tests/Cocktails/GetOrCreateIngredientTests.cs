namespace DrinksWeMake.Tests.Cocktails;

public class GetOrCreateIngredientTests
{
   [Fact]
   public async Task CreateNewIngredient_ReturnsCreatedIngredient()
   {
      await using var dbContext = TestDbContext.CreateDbContext();
   }
}