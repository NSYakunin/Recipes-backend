using Microsoft.EntityFrameworkCore;
using RecipesEFCore.DataAccess.SQLServer;
using RecipesEFCore3;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();


var connString = builder.Configuration.GetConnectionString("Data");

builder.Services.AddDbContext<RecipesEFCoreDbContext>(options => options.UseSqlServer(connString));
builder.Services.AddProblemDetails();
var app = builder.Build();

app.UseExceptionHandler();


app.MapPost("/recipes", async (RecipesEFCoreDbContext dbContext, RecipeDto recipeDto) =>
{
    // Валидация входных данных
    if (string.IsNullOrWhiteSpace(recipeDto.Name))
    {
        return Results.BadRequest("Recipe name is required.");
    }

    if (recipeDto.Ingredients == null || !recipeDto.Ingredients.Any())
    {
        return Results.BadRequest("At least one ingredient is required.");
    }

    foreach (var ingredientDto in recipeDto.Ingredients)
    {
        if (string.IsNullOrWhiteSpace(ingredientDto.Name))
        {
            return Results.BadRequest("Ingredient name is required.");
        }
    }

    // Маппинг DTO на сущности
    var recipe = new Recipe
    {
        Name = recipeDto.Name,
        IsVegetarian = recipeDto.IsVegetarian,
        IsVegan = recipeDto.IsVegan,
        Ingredients = recipeDto.Ingredients.Select(iDto => new Ingredient
        {
            Name = iDto.Name
        }).ToList()
    };

    await dbContext.Recipes.AddAsync(recipe);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/recipes/{recipe.RecipeID}", recipe);
});

app.Run();


public class RecipeDto
{
    [Required]
    public string Name { get; set; }
    public bool IsVegetarian { get; set; }
    public bool IsVegan { get; set; }

    [Required]
    public ICollection<IngredientDto>? Ingredients { get; set; }
}

public class IngredientDto
{
    [Required]
    public string Name { get; set; }
}
