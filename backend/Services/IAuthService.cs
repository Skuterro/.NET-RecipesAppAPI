using backend.DTOs;
using Microsoft.AspNetCore.Identity;

namespace backend.Services
{
    public class AuthServiceResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<IdentityError>? Errors { get; set; }
        public LoginResponseDTO? LoginResponse { get; set; }
    }
    public interface IAuthService
    {
        Task<AuthServiceResult> RegisterUserAsync(RegisterDTO registerDto);
        Task<AuthServiceResult> LoginUserAsync(LoginDTO loginDto);
    }
}
