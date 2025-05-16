using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Nazwa użytkownika lub email jest wymagana")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
