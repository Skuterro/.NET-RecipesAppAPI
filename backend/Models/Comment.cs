using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Treść komentarza jest wymagana.")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Komentarz musi zawierać od 1 do 500 znaków.")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ocena jest wymagana.")]
        [Range(1, 5, ErrorMessage = "Ocena musi być liczbą całkowitą od 1 do 5.")]
        public int Rating { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public Guid RecipeId { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
