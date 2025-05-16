using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
//TODO komunikaty na angielski
    public class Recipe
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Recipe name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nazwa musi zawierać od 3 do 100 znakow")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Category { get; set; }

        [Required(ErrorMessage = "Lista składników nie może być pusta.")]
        [MinLength(1, ErrorMessage = "Przepis musi zawierać co najmniej jeden składnik.")]
        public List<string> Ingredients { get; set; } = new List<string>();

        [Required(ErrorMessage = "Instrukcje przygotowania są wymagane.")]
        public string Instructions { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public  ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public double AverageRating { get; set; }

        public int RatingCount { get; set; }

    }
}
