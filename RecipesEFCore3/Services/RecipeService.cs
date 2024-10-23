using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipesEFCore.DataAccess.SQLServer;
using RecipesEFCore3.Models;
using RecipesEFCore3.Services;
using System;
using System.ComponentModel.DataAnnotations;

public class RecipeService : IRecipeService
{
    private readonly RecipesEFCoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public RecipeService(RecipesEFCoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    private bool TryValidateModel(object model, out List<ValidationResult> results)
    {
        var context = new ValidationContext(model, serviceProvider: null, items: null);
        results = new List<ValidationResult>();

        return Validator.TryValidateObject(model, context, results, validateAllProperties: true);
    }
    public async Task<IResult> CreateRecipeAsync(RecipeDto recipeDto)
    {
        // Валидация входных данных
        if (!TryValidateModel(recipeDto, out var errors))
        {
            return Results.BadRequest(errors);
        }

        // Маппинг RecipeDto на Recipe (без ингредиентов)
        var recipe = _mapper.Map<Recipe>(recipeDto);

        foreach (var ingredientDto in recipeDto.Ingredients)
        {
            // Проверяем, существует ли ингредиент в базе данных
            var ingredient = await _dbContext.Ingredients
                .FirstOrDefaultAsync(i => i.Name == ingredientDto.Name);

            if (ingredient == null)
            {
                // Если ингредиент не существует, создаём новый
                ingredient = new Ingredient { Name = ingredientDto.Name };
                _dbContext.Ingredients.Add(ingredient);
                await _dbContext.SaveChangesAsync();
            }

            // Добавляем связь в RecipeIngredients
            recipe.RecipeIngredients.Add(new RecipeIngredient
            {
                IngredientId = ingredient.IngredientId,
                Quantity = ingredientDto.Quantity,
                Unit = ingredientDto.Unit
            });
        }

        await _dbContext.Recipes.AddAsync(recipe);
        await _dbContext.SaveChangesAsync();

        // Формируем ответ с помощью DTO
        var recipeResponseDto = new RecipeResponseDto
        {
            RecipeID = recipe.RecipeID,
            Name = recipe.Name,
            IsVegetarian = recipe.IsVegetarian,
            IsVegan = recipe.IsVegan,
            Ingredients = recipe.RecipeIngredients.Select(ri => new IngredientDto
            {
                Name = ri.Ingredient.Name,
                Quantity = ri.Quantity,
                Unit = ri.Unit
            }).ToList()
        };

        return Results.Created($"/recipes/{recipe.RecipeID}", recipeResponseDto);
    }

    public async Task<IResult> GetRecipeByIdAsync(int id)
    {
        var recipes = await _dbContext.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.RecipeID == id);

        if (recipes == null)
        {
            return Results.NotFound();
        }

        var recipeResponseDto = _mapper.Map<RecipeResponseDto>(recipes);
        return Results.Ok(recipeResponseDto);
    }

    public async Task<IResult> GetRecipesAsync()
    {
        var recipes = await _dbContext.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .ToListAsync();

        var recipeResponseDtos = _mapper.Map<List<RecipeResponseDto>>(recipes);
        return Results.Ok(recipeResponseDtos);
    }

    public async Task<IResult> SearchRecipesAsync(string query)
    {
        var recipes = await _dbContext.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .Where(r => r.Name.ToLower().Contains(query.ToLower()) || r.RecipeIngredients.Any(ri => ri.Ingredient.Name.ToLower().Contains(query.ToLower())))
            .ToListAsync();

        var recipeResponseDto = _mapper.Map<List<RecipeResponseDto>>(recipes);
        return Results.Ok(recipeResponseDto);
    }

    public async Task<IResult> UpdateRecipeAsync(int id, RecipeDto recipeDto)
    {
        // Поиск рецепта в базе данных
        var recipe = await _dbContext.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.RecipeID == id);

        if (recipe == null)
        {
            return Results.NotFound();
        }

        // Маппинг RecipeDto на существующий Recipe (без ингредиентов)
        _mapper.Map(recipeDto, recipe);

        // Удаляем старые связи между рецептом и ингредиентами
        _dbContext.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);

        // Добавляем новые связи из DTO
        foreach (var ingredientDto in recipeDto.Ingredients)
        {
            // Проверяем, существует ли ингредиент в базе данных
            var ingredient = await _dbContext.Ingredients
                .FirstOrDefaultAsync(i => i.Name == ingredientDto.Name);

            if (ingredient == null)
            {
                // Если ингредиент не существует, создаём новый
                ingredient = new Ingredient { Name = ingredientDto.Name };
                _dbContext.Ingredients.Add(ingredient);
                await _dbContext.SaveChangesAsync();
            }

            // Добавляем новую связь в RecipeIngredients
            recipe.RecipeIngredients.Add(new RecipeIngredient
            {
                IngredientId = ingredient.IngredientId,
                Quantity = ingredientDto.Quantity,
                Unit = ingredientDto.Unit
            });
        }

        await _dbContext.SaveChangesAsync();

        return Results.Ok();
    }
    public async Task<IResult> DeleteRecipeAsync(int id)
    {
        var recipe = await _dbContext.Recipes
            .Include(r => r.RecipeIngredients)
            .FirstOrDefaultAsync(r => r.RecipeID == id);

        if (recipe == null)
        {
            return Results.NotFound();
        }

        // Удаляем все связи ингредиентов с рецептом
            _dbContext.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);

        // Удаляем сам рецепт
        _dbContext.Recipes.Remove(recipe);

        await _dbContext.SaveChangesAsync();

        return Results.Ok();
    }
}