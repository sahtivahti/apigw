namespace apigw.ExternalServices.RecipeService.Model
{
    public class CreateRecipeRequest
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string UserId { get; set; }
        public string Style { get; set; }
        public double BatchSize { get; set; }
    }
}
