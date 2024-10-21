using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniValidation;
using RecipesEFCore.DataAccess.SQLServer;
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
                await CreateRecipe(dbContext, mapper, recipeDto);
            });

            app.MapGet("/recipes/", () => GetRecipeById());
        }

        private static bool TryValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        }

        private static async Task<IResult> CreateRecipe(RecipesEFCoreDbContext dbContext, IMapper mapper, RecipeDto recipeDto)
        {

            // Валидация входных данных
            if (!TryValidateModel(recipeDto, out var errors))
            {
                return Results.BadRequest(errors);
            }

            // Маппинг RecipeDto на Recipe (без ингредиентов)
            var recipe = mapper.Map<Recipe>(recipeDto);

            foreach (var ingredientDto in recipeDto.Ingredients)
            {
                // Проверяем, существует ли ингредиент в базе данных
                var ingredient = await dbContext.Ingredients
                    .FirstOrDefaultAsync(i => i.Name == ingredientDto.Name);

                if (ingredient == null)
                {
                    // Если ингредиент не существует, создаём новый
                    ingredient = new Ingredient { Name = ingredientDto.Name };
                    dbContext.Ingredients.Add(ingredient);
                    await dbContext.SaveChangesAsync();
                }

                // Добавляем связь в RecipeIngredients
                recipe.RecipeIngredients.Add(new RecipeIngredient
                {
                    IngredientId = ingredient.IngredientId,
                    Quantity = ingredientDto.Quantity,
                    Unit = ingredientDto.Unit
                });
            }

            await dbContext.Recipes.AddAsync(recipe);
            await dbContext.SaveChangesAsync();

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


        private static async Task<IResult> GetRecipeById()
        {
            return Results.Redirect("https://www.google.com/search?q=%D1%85%D1%80%D0%B5%D0%BD&sca_esv=0ba5b4315e0bd468&sxsrf=ADLYWIJO25UAR0FIwEJLN_yMEMom3OEtvw%3A1729509985339&source=hp&ei=YToWZ92VEb65wPAP3PaVwQ0&iflsig=AL9hbdgAAAAAZxZIcc1NPOcslAx1hX0DJmP0jGjaAAxc&gs_ssp=eJzj4tDP1TdITsnOMWD04rjYerHhwtYLewFKAQjo&oq=%D0%A5%D1%80%D0%B5%D0%BD&gs_lp=Egdnd3Mtd2l6IgjQpdGA0LXQvSoCCAEyBRAAGIAEMg4QLhiABBixAxiDARjUAjIIEC4YgAQYsQMyCBAAGIAEGLEDMggQABiABBixAzIFEAAYgAQyBRAAGIAEMggQLhiABBixAzILEC4YgAQYxwEYrwEyBRAAGIAESIAgUOwLWNcQcAF4AJABAJgBnQGgAb4DqgEDMi4yuAEByAEA-AEBmAIFoALRA6gCCsICBxAjGCcY6gLCAgsQABiABBixAxiDAcICDhAAGIAEGLEDGIMBGIoFwgIFEC4YgATCAg4QLhiABBixAxiDARiKBcICCxAuGIAEGLEDGIMBmAMHkgcDMy4yoAevOw&sclient=gws-wiz");
            // Логика получения рецепта по ID
        }

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