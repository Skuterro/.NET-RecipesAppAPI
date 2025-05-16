namespace backend.DTOs
{
    public class CommentResponseDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid RecipeId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}
