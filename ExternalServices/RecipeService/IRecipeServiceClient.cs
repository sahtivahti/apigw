using System.Collections.Generic;
using System.Threading.Tasks;
using apigw.ExternalServices.RecipeService.Model;

namespace apigw.ExternalServices.RecipeService
{
    public interface IRecipeServiceClient
    {
        Task<RecipeDetailsResponse> GetById(int recipeId);
        Task<RecipeDetailsResponse> Create(CreateRecipeRequest request);
        Task<RecipeDetailsResponse> UpdateById(int recipeId, UpdateRecipeRequest request);
        Task DeleteById(int recipeId);
        Task<HopDetailsResponse> AddHopById(int recipeId, CreateHopRequest request);
        Task DeleteHopById(int recipeId, int hopId);
        Task<FermentableDetailsResponse> AddFermentableById(int recipeId, CreateFermentableRequest request);
        Task DeleteFermentableById(int recipeId, int fermentableId);
        Task<IEnumerable<RecipeListItem>> SearchRecipes(RecipeSearchFilters filters);
    }
}
