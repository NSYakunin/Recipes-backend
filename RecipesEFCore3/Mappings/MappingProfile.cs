using AutoMapper;
using RecipesEFCore3.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        /* Здесь указываем, что при маппинге RecipeDto на Recipe свойство Ingredients не будет автоматически сопоставляться.
           Это нужно, потому что мы вручную будем обрабатывать ингредиенты в логике создания рецепта (например, в CreateRecipe),
           И хотим избежать автоматической обработки. */
        CreateMap<RecipeDto, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.Ignore());

        CreateMap<IngredientDto, Ingredient>();

        /* Маппинг для ответа. Этот метод указывает, что при маппинге из Recipe в RecipeResponseDto
           Поле Ingredients будет заполняться данными из коллекции RecipeIngredients */
        CreateMap<Recipe, RecipeResponseDto>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.RecipeIngredients));


        /* Маппинг для преобразования сущности RecipeIngredient в IngredientDto.
           Это нужно для того, чтобы в ответе возвращать информацию об ингредиентах (например, их имя, количество и единицу измерения). */
        CreateMap<RecipeIngredient, IngredientDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Ingredient.Name))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit));
    }
}