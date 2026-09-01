using DrinksWeMake.Api.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Cocktail> Cocktails { get; set; }
    public DbSet<CocktailIngredient> CocktailIngredients { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Rating>()
            .HasIndex(r => new { r.UserId, r.CocktailId })
            .IsUnique();
    }
}