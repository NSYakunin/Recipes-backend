
using RecipesEFCore3.Models;
using System.ComponentModel.DataAnnotations;

public class RecipeDto
{
    [Required]
    public string Name { get; set; }
    public bool IsVegetarian { get; set; }
    public bool IsVegan { get; set; }

    [Required]
    public ICollection<IngredientDto> Ingredients { get; set; } // Список ингредиентов

    public ICollection<RecipeIngredient>? RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}