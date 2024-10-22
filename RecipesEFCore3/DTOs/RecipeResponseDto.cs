

public class RecipeResponseDto
{
    public int RecipeID { get; set; }
    public string Name { get; set; }
    public bool IsVegetarian { get; set; }
    public bool IsVegan { get; set; }
    public ICollection<IngredientDto>? Ingredients { get; set; }
}
