using RecipesEFCore3.DTOs;
using System.ComponentModel.DataAnnotations;

public class RecipeDto
{
    [Required]
    public string Name { get; set; }
    public bool IsVegetarian { get; set; }
    public bool IsVegan { get; set; }

    [Required]
    public ICollection<IngredientDto> Ingredients { get; set; } // Список ингредиентов
}