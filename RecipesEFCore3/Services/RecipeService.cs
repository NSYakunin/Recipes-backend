using AutoMapper;
using RecipesEFCore.DataAccess.SQLServer;
using RecipesEFCore3.DTOs;
using RecipesEFCore3.Models;
using RecipesEFCore3.Services;
using System;

public class RecipeService : IRecipeService
{
    private readonly RecipesEFCoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public RecipeService(RecipesEFCoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Recipe> CreateRecipeAsync(RecipeDto recipeDto)
    {
        var recipe = _mapper.Map<Recipe>(recipeDto);
        await _dbContext.Recipes.AddAsync(recipe);
        await _dbContext.SaveChangesAsync();
        return recipe;
    }
}