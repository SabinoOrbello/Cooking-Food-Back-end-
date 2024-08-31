namespace Cooking.Models
{
    public class RecipeDTO
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Instructions { get; set; }
        public int? CookingTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ImageUrl { get; set; }
    }
}
