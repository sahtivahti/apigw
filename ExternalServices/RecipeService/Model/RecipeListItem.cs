using System;

namespace apigw.ExternalServices.RecipeService.Model
{
    public class RecipeListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
