using System;
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

        public DateTime? CreatedAt { get; set; } = null;

        public DateTime? UpdatedAt { get; set; } = null;
    }
}
