using System.ComponentModel.DataAnnotations;

namespace apigw.Recipes
{
    public class Fermentable
    {
        public int? Id { get; set; } = null;

        [Required]
        public string Name { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double Color { get; set; }
    }
}
