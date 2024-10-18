using RecipesEFCore3.DTOs;
using RecipesEFCore3.Models;

namespace RecipesEFCore3.Services
{
    public interface IRecipeService
    {
        Task<Recipe> CreateRecipeAsync(RecipeDto recipeDto);
        // Другие методы
    }
}