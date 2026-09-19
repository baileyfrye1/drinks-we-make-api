using DrinksWeMake.Api.Common.Exceptions;
using DrinksWeMake.Api.Data.Entities;
using DrinksWeMake.Api.Features.Cocktails;
using DrinksWeMake.Tests.Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Tests.FeatureTests.Cocktails;

public class DeleteCocktailTests(DatabaseFixture dbFixture) : DatabaseTestBase(dbFixture)
{
    private ApplicationUser _user = null!;
    private Cocktail _cocktail = null!;
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "test@gmail.com",
            NormalizedUserName = "TEST@GMAIL.COM",
            Email = "test@gmail.com",
            NormalizedEmail = "TEST@GMAIL.COM",
            SecurityStamp =  Guid.NewGuid().ToString()
        };
        DbContext.Users.Add(_user);
        
        _cocktail = new Cocktail
        {
            Name = "Old Fashioned",
            Featured = true,
            UserId = _user.Id,
            CocktailIngredients = new List<CocktailIngredient>
            {
                new()
                {
                    Ingredient = new Ingredient
                    {
                      Name = "Bourbon",
                      CreatedAt = DateTime.UtcNow
                    },
                    Amount = 2,
                    Unit = "oz",
                    Float = false,
                    CreatedAt = DateTime.UtcNow,   
                },
                new()
                {
                    Ingredient = new Ingredient
                    {
                        Name = "Simple Syrup",
                        CreatedAt = DateTime.UtcNow
                    },
                    Amount = 0.25,
                    Unit = "oz",
                    Float = false,
                    CreatedAt = DateTime.UtcNow,   
                },
                new()
                {
                    Ingredient = new Ingredient
                    {
                        Name = "Angostura Bitters",
                        CreatedAt = DateTime.UtcNow
                    },
                    Amount = 2,
                    Unit = "dashes",
                    Float = false,
                    CreatedAt = DateTime.UtcNow,   
                },
            },
            Ratings = new List<Rating>
            {
                new()
                {
                    RatingValue = 5,
                    UserId = _user.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        DbContext.Cocktails.Add(_cocktail);
        
        await DbContext.SaveChangesAsync();
    }
    
    [Fact]
    public async Task DeleteOwnedCocktail_DeletesCocktail()
    {
        // Arrange
        (await DbContext.Cocktails.CountAsync()).Should().Be(1);
        
        // Act
        await DeleteCocktail.Handle(_user.Id, DbContext, _cocktail.Id, CancellationToken.None);
        
        // Assert
        (await DbContext.Cocktails.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task TryDeleteCocktailOwnedByAnotherUser_ThrowsNotFoundException()
    {
       // Arrange
        var newUser = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "test1@gmail.com",
            NormalizedUserName = "TEST1@GMAIL.COM",
            Email = "test1@gmail.com",
            NormalizedEmail = "TEST1@GMAIL.COM",
            SecurityStamp =  Guid.NewGuid().ToString()
        };
        
        DbContext.Users.AddRange(newUser);
        await DbContext.SaveChangesAsync();
       
       // Act
       var act = async () => await DeleteCocktail.Handle(newUser.Id, DbContext, _cocktail.Id, CancellationToken.None);

       // Assert
       await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task TryDeleteNonExistentCocktail_ThrowsNotFoundException()
    {
       var act = async () => await DeleteCocktail.Handle(_user.Id, DbContext, 5, CancellationToken.None);
       
       await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteCocktailWithIngredients_DeleteCascades()
    {
        (await DbContext.CocktailIngredients.Where(ci => ci.CocktailId == _cocktail.Id).CountAsync()).Should().NotBe(0);
        
        await DeleteCocktail.Handle(_user.Id, DbContext, _cocktail.Id, CancellationToken.None);
        
        (await DbContext.CocktailIngredients.Where(ci => ci.CocktailId == _cocktail.Id).CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task DeleteCocktailWithRatings_DeleteCascades()
    {
        (await DbContext.Ratings.CountAsync()).Should().NotBe(0);
        
        await DeleteCocktail.Handle(_user.Id, DbContext, _cocktail.Id, CancellationToken.None);
        
        (await DbContext.Ratings.CountAsync()).Should().Be(0);
    }
}