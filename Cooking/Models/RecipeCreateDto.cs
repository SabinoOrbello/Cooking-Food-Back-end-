namespace Cooking.Models
{
    public class RecipeCreateDto
    {
        public string Title { get; set; }
        public string Instructions { get; set; }
        public string Description { get; set; }
        public int CookingTime { get; set; } // Assicurati che sia un int, o long se hai bisogno di supportare numeri più grandi
        public string ImageUrl { get; set; }
        public List<int> IngredientIds { get; set; }
        public List<int> CategoriyId { get; set; }
    }
}
