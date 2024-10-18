using RecipesEFCore3.DTOs;
using System.ComponentModel.DataAnnotations;

public class RecipeDto
{
    [Required]
    public string Name { get; set; }
    public bool IsVegetarian { get; set; }
    public bool IsVegan { get; set; }
    [Required]
    [MinLength(1, ErrorMessage = "At least one ingredient is required.")]
    public ICollection<IngredientDto> Ingredients { get; set; }
}