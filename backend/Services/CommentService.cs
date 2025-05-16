using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _dbContext;

        public CommentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CommentResponseDTO>> GetCommentsForRecipeAsync(Guid recipeId)
        {
            return await _dbContext.Comments
                .Include(c => c.User)
                .Where(c => c.RecipeId == recipeId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentResponseDTO
                {
                    Id = c.Id,
                    Text = c.Text,
                    Rating = c.Rating,
                    CreatedAt = c.CreatedAt,
                    RecipeId = c.RecipeId,
                    UserId = c.UserId,
                    Author = c.User.UserName ?? "Anonymous"
                })
                .AsNoTracking().ToListAsync();
        }
        public async Task<CommentResponseDTO?> AddCommentAsync(Guid recipeId, string userId, CreateCommentDTO createDto)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                return null;
            }
            
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Text = createDto.Text,
                Rating = createDto.Rating,
                CreatedAt = DateTime.UtcNow,
                RecipeId = recipeId,
                UserId = userId
            };

            _dbContext.Comments.Add(comment);

            double currentRatingValue = recipe.AverageRating * recipe.RatingCount;
            recipe.RatingCount++;
            recipe.AverageRating = (currentRatingValue + comment.Rating) / recipe.RatingCount;

            await _dbContext.SaveChangesAsync();

            return new CommentResponseDTO
            {
                Id = comment.Id,
                Text = comment.Text,
                Rating = comment.Rating,
                CreatedAt = comment.CreatedAt,
                RecipeId = comment.RecipeId,
                UserId = comment.UserId,
                Author = user.UserName ?? "Anonymous"
            };
        }

        public async Task<bool> DeleteCommentAsync(Guid commentId, string userId)
        {
            var comment = await _dbContext.Comments
                .Include(c => c.Recipe)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return false;
            }

            if(comment.UserId != userId)
            {
                return false;
            }

            var recipe = comment.Recipe;
            _dbContext.Comments.Remove(comment);

            double currentRatingValue = recipe.AverageRating * recipe.RatingCount;
            recipe.RatingCount--;

            if (recipe.RatingCount > 0)
            {
                recipe.AverageRating = (currentRatingValue - comment.Rating) / recipe.RatingCount;
            }
            else
            {
                recipe.AverageRating = 0;
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
