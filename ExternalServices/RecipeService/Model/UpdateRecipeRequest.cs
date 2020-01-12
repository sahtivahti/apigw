namespace apigw.ExternalServices.RecipeService.Model
{
    public class UpdateRecipeRequest
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Style { get; set; }
        public double BatchSize { get; set; }
    }
}
