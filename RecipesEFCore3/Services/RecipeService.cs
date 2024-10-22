using AutoMapper;
using RecipesEFCore.DataAccess.SQLServer;
using RecipesEFCore3.Models;
using RecipesEFCore3.Services;
using System;

public class RecipeService : IRecipeService
{
    private readonly RecipesEFCoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public Task CreateRecipeAsync(RecipeDto recipeDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRecipeAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<RecipeResponseDto> GetRecipeByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<RecipeResponseDto>> GetRecipesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<RecipeResponseDto>> SearchRecipesAsync(string query)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecipeAsync(int id, RecipeDto recipeDto)
    {
        throw new NotImplementedException();
    }
}