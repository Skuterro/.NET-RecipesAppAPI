using System.Runtime.CompilerServices;
using backend.DTOs;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            this._recipeService = recipeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetAllRecipes()
        {
            var recipes = await _recipeService.GetAllAsync();
            return Ok(recipes);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RecipeResponseDTO>> GetRecipeById(Guid id)
        {
            var recipe = await _recipeService.GetByIdAsync(id);
            if (recipe == null)
            {
                return NotFound($"Recipe with ID: {id} not found");
            }
            return Ok(recipe);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetLoggedUserRecipes()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User cannot be identified.");
            }

            var recipes = await _recipeService.GetUserRecipesAsync(userId);
            return Ok(recipes);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Recipe>> CreateRecipe([FromBody] CreateRecipeDTO newRecipe)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User cannot be identified.");
            }

            var createdRecipe = await _recipeService.CreateAsync(newRecipe, userId);

            return CreatedAtAction(nameof(GetRecipeById), new { id = createdRecipe.Id }, createdRecipe);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateRecipe(Guid id, [FromBody] UpdateRecipeDTO updatedRecipe)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _recipeService.UpdateAsync(id, updatedRecipe);

            if (!success)
            {
                return NotFound($"Recipe with ID: {id} not found");
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteRecipe(Guid id)
        {
            var success = await _recipeService.DeleteAsync(id);
            if (!success)
            {
                return NotFound($"Recipe with ID: {id} not found");
            }
            return NoContent();
        }
    }
}
