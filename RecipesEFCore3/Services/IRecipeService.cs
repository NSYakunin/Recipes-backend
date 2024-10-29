
using RecipesEFCore3.Models;

namespace RecipesEFCore3.Services
{
    public interface IRecipeService
    {
        Task<IResult> CreateRecipeAsync(RecipeDto recipeDto);
        Task<IResult> GetRecipesAsync(int? page, int? pageSize);
        Task<IResult> GetRecipeByIdAsync(int id);
        Task<IResult> SearchRecipesAsync(string query, int? page, int? pageSize);
        Task<IResult> UpdateRecipeAsync(int id, RecipeDto recipeDto);
        Task<IResult> DeleteRecipeAsync(int id);
    }
}