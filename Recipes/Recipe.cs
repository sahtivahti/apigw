using System.ComponentModel.DataAnnotations;

namespace apigw.Recipes
{
    public class Recipe
    {
        public int? Id { get; set; }

        [Required]
        [EmailAddress]
        public string Author { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
