using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Wstrzyknij IAuthService
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterUserAsync(registerDto);

            if (!result.Succeeded)
            {
                var errors = result.Errors?.Select(e => e.Description) ?? new List<string> { "An unknown error occurred during registration." };
                return BadRequest(new { Message = "Registration failed!", Errors = errors });
            }

            return Ok(new { Message = "Registration completed successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginUserAsync(loginDto);

            if (!result.Succeeded || result.LoginResponse == null)
            {
                return Unauthorized(new { Message = "Invalid username/email or password." });
            }

            return Ok(result.LoginResponse);
        }
    }
}
