
using RecipesEFCore3.Models;

namespace RecipesEFCore3.Services
{
    public interface IRecipeService
    {
        Task CreateRecipeAsync(RecipeDto recipeDto);
        Task<List<RecipeResponseDto>> GetRecipesAsync();
        Task<RecipeResponseDto> GetRecipeByIdAsync(int id);
        Task<List<RecipeResponseDto>> SearchRecipesAsync(string query);
        Task UpdateRecipeAsync(int id, RecipeDto recipeDto);
        Task DeleteRecipeAsync(int id);
    }
}