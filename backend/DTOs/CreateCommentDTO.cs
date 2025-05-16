using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CreateCommentDTO
    {

        [Required(ErrorMessage = "Treść komentarza jest wymagana.")]
        [StringLength(500, MinimumLength = 1)]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ocena jest wymagana.")]
        [Range(1, 5, ErrorMessage = "Ocena musi być liczbą całkowitą od 1 do 5.")]
        public int Rating { get; set; }

    }
}
