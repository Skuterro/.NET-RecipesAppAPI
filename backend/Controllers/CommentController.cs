using System.Security.Claims;
using backend.DTOs;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IRecipeService _recipeService;

        public CommentController(ICommentService commentService, IRecipeService recipeService)
        {
            _commentService = commentService;
            _recipeService = recipeService;
        }

        [HttpGet("recipes/{recipeId:guid}/comments")]
        public async Task<IActionResult> GetComments(Guid recipeId)
        {
            var recipe = await _recipeService.GetByIdAsync(recipeId);
            if (recipe == null)
            {
                return NotFound($"Recipe with ID: {recipeId} not found");
            }

            var comments = await _commentService.GetCommentsForRecipeAsync(recipeId);
            return Ok(comments);
        }

        [HttpPost("recipes/{recipeId:guid}/comments")]
        [Authorize]
        public async Task<IActionResult> PostComment(Guid recipeId, [FromBody] CreateCommentDTO createComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User cannot be identified.");
            }

            var createdCommentDto = await _commentService.AddCommentAsync(recipeId, userId, createComment);
            if (createdCommentDto == null)
            {
                return NotFound($"Recipe with ID: {recipeId} not found");
            }

            return Created($"api/comments/{createdCommentDto.Id}", createdCommentDto);
        }

        [HttpDelete("comments/{id:guid}")]
        [Authorize] // Wymaga zalogowania
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User cannot be identified.");
            }
            var success = await _commentService.DeleteCommentAsync(id, userId);
            if (!success)
            {
                return NotFound($"Comment with ID: {id} not found");
            }
            return NoContent();
            
        }
    }
}
