using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesEFCore3.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public required string Name { get; set; }

        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    }
}
