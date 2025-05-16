using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthServiceResult> RegisterUserAsync(RegisterDTO registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.Username);
            if (userExists != null)
            {
                return new AuthServiceResult { Succeeded = false, Errors = new List<IdentityError> { new IdentityError { Code = "UsernameExists", Description = "Użytkownik o tej nazwie już istnieje!" } } };
            }
            var emailExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (emailExists != null)
            {
                return new AuthServiceResult { Succeeded = false, Errors = new List<IdentityError> { new IdentityError { Code = "EmailExists", Description = "Adres email jest już zajęty!" } } };
            }


            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            return new AuthServiceResult { Succeeded = result.Succeeded, Errors = result.Errors };
        }

        public async Task<AuthServiceResult> LoginUserAsync(LoginDTO loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Login) ?? await _userManager.FindByEmailAsync(loginDto.Login);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var token = GenerateJwtToken(user);
                var tokenHandler = new JwtSecurityTokenHandler();

                return new AuthServiceResult
                {
                    Succeeded = true,
                    LoginResponse = new LoginResponseDTO
                    {
                        Token = tokenHandler.WriteToken(token),
                        Expiration = token.ValidTo,
                        Username = user.UserName ?? "N/A",
                        Email = user.Email ?? "N/A"
                    }
                };
            }

            return new AuthServiceResult { Succeeded = false };
        }

        private JwtSecurityToken GenerateJwtToken(IdentityUser user)
        {
            var jwtKey = _configuration["JWT_KEY"];
            var jwtIssuer = _configuration["JWT_ISSUER"];
            var jwtAudience = _configuration["JWT_AUDIENCE"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("Konfiguracja JWT (Key, Issuer, Audience) nie jest poprawnie ustawiona.");
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
