namespace backend.DTOs
{
    public class RecipeResponseDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public List<string> Ingredients { get; set; } = new List<string>();
        public string Instructions { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public List<CommentResponseDTO> Comments { get; set; } = new List<CommentResponseDTO>();
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
    }
}
