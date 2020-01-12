using System;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using apigw.Recipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using apigw.Recipes.Model;

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
        public async Task<IEnumerable<RecipeListItem>> GetAllRecipes()
        {
            return await _recipeService.GetRecipes();
        }

        [HttpPost("/v1/recipe")]
        [Authorize]
        public async Task<ActionResult<RecipeDetails>> CreateRecipe([FromBody] Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            recipe.UserId = userId;

            var result = await _recipeService.CreateRecipe(recipe);

            return Created(
                "Recipe",
                await _recipeService.GetRecipeById(
                    result.Id ?? throw new InvalidOperationException("Can't load recipe without id")
                )
            );
        }

        [HttpPut("/v1/recipe/{id}")]
        public async Task<ActionResult<RecipeDetails>> UpdateRecipe([FromBody] Recipe recipe, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            recipe.Id = id;

            try
            {
                await _recipeService.UpdateRecipe(recipe);

                return Ok(await _recipeService.GetRecipeById(id));
            }
            catch (ExternalServices.RecipeService.RecipeNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("/v1/recipe/{id}")]
        public async Task<ActionResult<RecipeDetails>> GetRecipeDetails(int id)
        {
            try
            {
                return Ok(await _recipeService.GetRecipeById(id));
            }
            catch (ExternalServices.RecipeService.RecipeNotFoundException)
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
            catch (ExternalServices.RecipeService.RecipeNotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost("/v1/recipe/{recipeId}/hop")]
        public async Task<ActionResult<Recipes.Hop>> AddHopToRecipe([FromBody] Recipes.Hop hop, int recipeId)
        {
            try
            {
                var result = await _recipeService.AddHopToRecipe(hop, recipeId);

                return Created("", result);
            }
            catch (ExternalServices.RecipeService.RecipeNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("/v1/recipe/{recipeId}/hop/{hopId}")]
        public async Task<IActionResult> RemoveHopFromRecipe(int recipeId, int hopId)
        {
            try
            {
                await _recipeService.RemoveHopFromRecipe(hopId, recipeId);

                return Ok();
            }
            catch (ExternalServices.RecipeService.RecipeNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("/v1/recipe/{recipeId}/fermentable")]
        public async Task<ActionResult<Recipes.Fermentable>> AddFermentableToRecipe([FromBody] Recipes.Fermentable fermentable, int recipeId)
        {
            try
            {
                var result = await _recipeService.AddFermentableToRecipe(fermentable, recipeId);

                return Created("", result);
            }
            catch (ExternalServices.RecipeService.RecipeNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("/v1/recipe/{recipeId}/fermentable/{fermentableId}")]
        public async Task<IActionResult> RemoveFermentableFromRecipe(int recipeId, int fermentableId)
        {
            try
            {
                await _recipeService.RemoveFermentableFromRecipe(fermentableId, recipeId);

                return Ok();
            }
            catch (ExternalServices.RecipeService.RecipeNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
