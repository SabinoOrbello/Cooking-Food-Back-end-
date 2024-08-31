using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cooking.Models;

namespace Cooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeContext _context;

        public RecipeController(RecipeContext context)
        {
            _context = context;
        }

        // GET: api/Recipe?ingredientIds=1&ingredientIds=2&ingredientIds=3
        [HttpGet]
        public async Task<IActionResult> GetRecipes([FromQuery] List<int> ingredientIds)
        {
            if (ingredientIds == null || !ingredientIds.Any())
            {
                return BadRequest("Ingredient IDs are required.");
            }

            // Recupera le ricette che contengono uno degli ingredienti specificati
            var recipes = await _context.Recipes
                .Where(r => r.Ingredients.Any(i => ingredientIds.Contains(i.IngredientId)))
                .ToListAsync();

            if (recipes == null || !recipes.Any())
            {
                return NotFound("No recipes found for the provided ingredient IDs.");
            }

            return Ok(recipes);
        }

        // GET: api/Recipe/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        // PUT: api/Recipe/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return BadRequest();
            }

            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Recipe
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipe", new { id = recipe.RecipeId }, recipe);
        }

        // DELETE: api/Recipe/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("add-recipe")]
        public async Task<IActionResult> AddRecipe([FromBody] RecipeCreateDto request)
        {
            // Creazione di una nuova entità Ricetta
            var recipe = new Recipe
            {
                Title = request.Title,
                Instructions = request.Instructions,
                Description = request.Description, // Aggiunto
                CookingTime = request.CookingTime, // Aggiunto
                ImageUrl = request.ImageUrl, // Aggiunto
                Ingredients = new List<Ingredient>(),
                Categories = new List<Category>()
            };

            // Recupera gli ingredienti selezionati dal database
            var selectedIngredients = await _context.Ingredients
                .Where(i => request.IngredientIds.Contains(i.IngredientId))
                .ToListAsync();

            // Aggiungi gli ingredienti selezionati alla nuova ricetta
            recipe.Ingredients = selectedIngredients;

            // Aggiunta della nuova ricetta al contesto
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok(new { recipe.RecipeId, recipe.Title });
        }

        [HttpGet("search-by-ingredients")]
        public async Task<ActionResult<IEnumerable<Recipe>>> SearchByIngredients([FromQuery] List<int> ingredientIds)
        {
            if (ingredientIds == null || ingredientIds.Count == 0)
            {
                return BadRequest("No ingredients specified.");
            }

            try
            {
                // Recupera tutte le ricette che contengono almeno uno degli ingredienti specificati
                var recipes = await _context.Recipes
                    .Where(r => r.Ingredients.Any(i => ingredientIds.Contains(i.IngredientId)))
                    .ToListAsync();

                if (recipes == null || !recipes.Any())
                {
                    return NotFound("No recipes found for the specified ingredients.");
                }

                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.RecipeId == id);
        }
    }
}
