using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CreateRecipeDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public List<string> Ingredients { get; set; } = new List<string>();
        public string Instructions {  get; set; } = string.Empty;
    }
}
