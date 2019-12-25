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
        public async Task<IEnumerable<Recipe>> ListRecipes()
        {
            return await _recipeService.GetRecipes();
        }
    }
}
