using AutoMapper;
using RecipesEFCore3.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RecipeDto, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.Ignore());

        CreateMap<IngredientDto, Ingredient>();

        // Маппинг для ответа
        CreateMap<Recipe, RecipeResponseDto>();
        CreateMap<Ingredient, IngredientDto>();
    }
}