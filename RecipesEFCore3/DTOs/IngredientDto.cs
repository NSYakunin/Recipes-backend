using System.ComponentModel.DataAnnotations;

namespace RecipesEFCore3.DTOs
{
    public class IngredientDto
    {
        [Required]
        public string Name { get; set; }
    }
}
