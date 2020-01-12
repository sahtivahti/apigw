using System.Threading.Tasks;
using apigw.Recipes;

namespace apigw.ExternalServices.BeerCalculator
{
    public interface IBeerCalculator
    {
        Task<CalculationResponse> GetForRecipe(Recipe recipe);
        Task<CalculationResponse> Calculate(CalculationRequest request);
    }
}
