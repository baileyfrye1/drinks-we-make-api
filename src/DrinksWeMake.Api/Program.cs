using System.Text.Json;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
using DrinksWeMake.Api.Extensions;
using DrinksWeMake.Api.Features.Cocktails;
using DrinksWeMake.Api.Features.Cocktails.CreateCocktail;
using DrinksWeMake.Api.Features.Cocktails.UpdateCocktail;
using DrinksWeMake.Api.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(
    options => 
        options
            .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            .UseSnakeCaseNamingConvention()
        );

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

builder.Services.AddHttpClient<IStorageClient, SupabaseStorageClient>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["Storage:Url"]);
        
        var serviceKey = builder.Configuration["Storage:Key"];
        
        client.DefaultRequestHeaders.Add("apiKey", serviceKey);
    }
);

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorization();

builder.Services.AddDependencies();

var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

// Prefix auth endpoints
var authGroup = app.MapGroup("/v1/auth");
authGroup.MapIdentityApi<ApplicationUser>();

app.MapAppEndpoints();

app.Run();
