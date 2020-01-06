using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using apigw.Recipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using apigw.BeerCalculator;

namespace apigw.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly IBeerCalculator _calculator;

        public RecipeController(IRecipeService recipeService, IBeerCalculator calculator)
        {
            _recipeService = recipeService;
            _calculator = calculator;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            recipe.Id = id;

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
            var response = new RecipeDetailsResponse();

            Recipe recipe;

            try
            {
                recipe = await _recipeService.GetRecipeById(id);

                response.Apply(recipe);
            }
            catch (RecipeNotFoundException)
            {
                return NotFound();
            }

            // Calculate recipe information
            response.Apply(await _calculator.GetForRecipe(recipe));

            return Ok(response);
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
        public async Task<IActionResult> AddHopToRecipe([FromBody] Recipes.Hop hop, int recipeId)
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

        [HttpPost("/v1/recipe/{recipeId}/fermentable")]
        public async Task<IActionResult> AddFermentableToRecipe([FromBody] Recipes.Fermentable fermentable, int recipeId)
        {
            try
            {
                var result = await _recipeService.AddFermentableToRecipe(fermentable, recipeId);

                return Created("", result);
            }
            catch (RecipeNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("/v1/recipe/{recipeId}/fermentable/{fermentableId}")]
        public async Task<IActionResult> RemoveFermentableFromRecipe(int recipeId, int fermentableId)
        {
            try
            {
                var result = await _recipeService.RemoveFermentableFromRecipe(fermentableId, recipeId);

                return Ok(result);
            }
            catch (RecipeNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
