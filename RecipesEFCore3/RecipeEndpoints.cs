using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using RecipesEFCore.DataAccess.SQLServer;
using RecipesEFCore3.DTOs;
using RecipesEFCore3.Models;
using RecipesEFCore3.Services;
using System;

namespace RecipesEFCore3.Endpoints
{
    public static class RecipeEndpoints
    {
        public static void MapRecipeEndpoints(this WebApplication app)
        {
            app.MapPost("/recipes", CreateRecipe);

            //app.MapGet("/recipes/{id}", GetRecipeById);
            //app.MapPut("/recipes/{id}", UpdateRecipe);
            //app.MapDelete("/recipes/{id}", DeleteRecipe);
        }

        private static async Task<IResult> CreateRecipe(IRecipeService recipeService, [FromBody] RecipeDto recipeDto)
        {
            if (!MiniValidator.TryValidate(recipeDto, out var errors))
            {
                return Results.BadRequest(errors);
            }

            var recipe = await recipeService.CreateRecipeAsync(recipeDto);
            return Results.Created($"/recipes/{recipe.RecipeID}", recipe);
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