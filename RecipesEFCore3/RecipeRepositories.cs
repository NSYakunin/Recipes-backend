using Microsoft.EntityFrameworkCore;
using RecipesEFCore.DataAccess.SQLServer;
using System;

namespace RecipesEFCore3;


public class RecipeRepositories
{
    private readonly RecipesEFCoreDbContext _dbContext;

    public RecipeRepositories(RecipesEFCoreDbContext dbContext)
    {
        dbContext = _dbContext;
    }

    public async Task<List<Recipe>> GetRecipe()
    {
        return await _dbContext.Recipes.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<List<Recipe>> GetRecipeWithIngredient()
    {
        return await _dbContext.Recipes
            .AsNoTracking()
            .ToListAsync();
    }

}
