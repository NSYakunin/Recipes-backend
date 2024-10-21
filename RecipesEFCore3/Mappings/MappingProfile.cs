using AutoMapper;
using RecipesEFCore3.Models;
using RecipesEFCore3.DTOs;

namespace RecipesEFCore3.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RecipeDto, Recipe>()
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore());

            CreateMap<IngredientDto, Ingredient>()
                .ForMember(dest => dest.IngredientId, opt => opt.Ignore());
        }
    }
}