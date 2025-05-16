using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class UpdateRecipeDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new List<string>();
        public string Instructions { get; set; } = string.Empty;
    }
}
