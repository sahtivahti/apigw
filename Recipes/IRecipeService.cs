using System.Collections.Generic;
using System.Threading.Tasks;
using apigw.Recipes.Model;

namespace apigw.Recipes
{
    public interface IRecipeService
    {
        public Task<IEnumerable<RecipeListItem>> GetRecipes();
        public Task<RecipeDetails> GetRecipeById(int id);
        public Task<Recipe> UpdateRecipe(Recipe recipe);
        public Task<Recipe> CreateRecipe(Recipe recipe);
        public Task RemoveRecipeById(int id);
        public Task<Hop> AddHopToRecipe(Hop hop, int recipeId);
        public Task RemoveHopFromRecipe(int hopId, int recipeId);
        public Task<Fermentable> AddFermentableToRecipe(Fermentable fermentable, int recipeId);
        public Task RemoveFermentableFromRecipe(int fermentableId, int recipeId);
    }
}
