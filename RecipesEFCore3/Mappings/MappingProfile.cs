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
        CreateMap<Recipe, RecipeResponseDto>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.RecipeIngredients));

        CreateMap<RecipeIngredient, IngredientDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Ingredient.Name))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit));
    }
}