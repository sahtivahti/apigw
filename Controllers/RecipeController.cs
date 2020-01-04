using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using apigw.Recipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apigw.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet("/v1/recipe")]
        [Authorize]
        public async Task<IEnumerable<Recipe>> GetAllRecipes()
        {
            return await _recipeService.GetRecipes();
        }

        [HttpPost("/v1/recipe")]
        [Authorize]
        public async Task<IActionResult> CreateRecipe([FromBody] Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            recipe.UserId = userId;

            return Created("Recipe", await _recipeService.CreateRecipe(recipe));
        }

        [HttpPut("/v1/recipe/{id}")]
        public async Task<IActionResult> UpdateRecipe([FromBody] Recipe recipe, int id)
        {
            recipe.Id = id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _recipeService.UpdateRecipe(recipe);

                return Ok(result);
            }
            catch (RecipeNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("/v1/recipe/{id}")]
        public async Task<IActionResult> GetRecipeDetails(int id)
        {
            try
            {
                var recipe = await _recipeService.GetRecipeById(id);

                return Ok(recipe);
            }
            catch (RecipeNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("/v1/recipe/{id}")]
        public async Task<IActionResult> RemoveRecipe(int id)
        {
            try
            {
                await _recipeService.RemoveRecipeById(id);
            }
            catch (RecipeNotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost("/v1/recipe/{recipeId}/hop")]
        public async Task<IActionResult> AddHopToRecipe([FromBody] Hop hop, int recipeId)
        {
            try
            {
                var result = await _recipeService.AddHopToRecipe(hop, recipeId);

                return Created("", result);
            }
            catch (RecipeNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("/v1/recipe/{recipeId}/hop/{hopId}")]
        public async Task<IActionResult> RemoveHopFromRecipe(int recipeId, int hopId)
        {
            try
            {
                var result = await _recipeService.RemoveHopFromRecipe(hopId, recipeId);

                return Ok(result);
            }
            catch (RecipeNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
