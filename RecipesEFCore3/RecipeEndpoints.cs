using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniValidation;
using RecipesEFCore.DataAccess.SQLServer;
using RecipesEFCore3.DTOs;
using RecipesEFCore3.Models;
using RecipesEFCore3.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace RecipesEFCore3.Endpoints
{
    public static class RecipeEndpoints
    {
        public static void MapRecipeEndpoints(this WebApplication app)
        {
            app.MapPost("/recipes", async (RecipesEFCoreDbContext dbContext, IMapper mapper, RecipeDto recipeDto) =>
            {
                // Валидация входных данных
                if (!TryValidateModel(recipeDto, out var errors))
                {
                    return Results.BadRequest(errors);
                }

                // Маппинг RecipeDto на Recipe (без ингредиентов)
                var recipe = mapper.Map<Recipe>(recipeDto);

                // Обработка ингредиентов
                var ingredientNames = recipeDto.Ingredients.Select(i => i.Name.Trim().ToLower()).Distinct().ToList();

                // Проверяем, какие ингредиенты уже существуют
                var existingIngredients = await dbContext.Ingredients
                    .Where(i => ingredientNames.Contains(i.Name.ToLower()))
                    .ToListAsync();

                // Определяем, какие ингредиенты из нашего списка отсутствуют в базе данных, и создаём объекты для них
                //ingredientNames.Except(...) — находим имена ингредиентов, которые есть в ingredientNames, но отсутствуют среди existingIngredients
                var newIngredientNames = ingredientNames.Except(existingIngredients.Select(i => i.Name.ToLower())).ToList();
                var newIngredients = newIngredientNames.Select(name => new Ingredient { Name = name }).ToList();

                // Если есть новые ингредиенты, добавляем их в базу данных.
                if (newIngredients.Any())
                {
                    await dbContext.Ingredients.AddRangeAsync(newIngredients);
                    await dbContext.SaveChangesAsync();
                }

                // Объединяем существующие и новые ингредиенты
                var allIngredients = existingIngredients.Concat(newIngredients).ToList();

                // Связываем ингредиенты с рецептом
                recipe.Ingredients = allIngredients;

                await dbContext.Recipes.AddAsync(recipe);
                await dbContext.SaveChangesAsync();

                return Results.Created($"/recipes/{recipe.RecipeID}", recipe);
            });
            }

        public static bool TryValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        }


        //private static async Task<IResult> GetRecipeById(RecipesEFCoreDbContext dbContext, int id)
        //{
        //    // Логика получения рецепта по ID
        //}

        //private static async Task<IResult> UpdateRecipe(RecipesEFCoreDbContext dbContext, int id, RecipeDto recipeDto)
        //{
        //    // Логика обновления рецепта
        //}

        //private static async Task<IResult> DeleteRecipe(RecipesEFCoreDbContext dbContext, int id)
        //{
        //    // Логика удаления рецепта
        //}
    }
}