using System;
using System.Collections.Generic;

namespace Cooking.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Instructions { get; set; }

    public int? CookingTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
