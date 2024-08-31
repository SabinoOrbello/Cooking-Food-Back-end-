﻿using System;
using System.Collections.Generic;


namespace Cooking.Models;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string Name { get; set; } = null!;


    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
