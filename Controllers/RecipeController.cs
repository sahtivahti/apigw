using System;
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
        public async Task<Recipe> CreateRecipe([FromBody] Recipe recipe)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (email == null)
            {
                throw new ArgumentNullException("User email");
            }

            recipe.Author = email;

            return await _recipeService.CreateRecipe(recipe);
        }
    }
}
