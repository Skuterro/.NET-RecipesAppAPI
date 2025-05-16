using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly AppDbContext _appDbContext;

        public RecipeService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return await _appDbContext.Recipes.AsNoTracking().ToListAsync();
        }

        public async Task<RecipeResponseDTO?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Recipes
             .Where(r => r.Id == id)
             .Include(r => r.User)
             .Include(r => r.Comments)
                 .ThenInclude(c => c.User)
             .Select(r => new RecipeResponseDTO
             {
                 Name = r.Name,
                 Description = r.Description,
                 Category = r.Category,
                 Ingredients = r.Ingredients,
                 Instructions = r.Instructions,
                 UserId = r.UserId,
                 Author = r.User.UserName ?? "Anonymous",
                 AverageRating = r.AverageRating,
                 RatingCount = r.RatingCount,
                 Comments = r.Comments.OrderBy(c => c.CreatedAt).Select(c => new CommentResponseDTO
                 {
                     Id = c.Id,
                     Text = c.Text,
                     Rating = c.Rating,
                     CreatedAt = c.CreatedAt,
                     RecipeId = c.RecipeId,
                     UserId = c.UserId,
                     Author = c.User.UserName ?? "Anonymous"
                 }).ToList()
             })
             .AsNoTracking()
             .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Recipe>> GetUserRecipesAsync(string userId)
        {
            return await _appDbContext.Recipes.Where(r => r.UserId == userId).ToListAsync();
               
        }
        public async Task<Recipe> CreateAsync(CreateRecipeDTO newRecipe, string userId)
        {
            var recipe = new Recipe()
            {
                Id = Guid.NewGuid(),
                Name = newRecipe.Name,
                Description = newRecipe.Description,
                Category = newRecipe.Category,
                Ingredients = newRecipe.Ingredients,
                Instructions = newRecipe.Instructions,
                UserId = userId,
                AverageRating = 0,
                RatingCount = 0
            };
            _appDbContext.Recipes.Add(recipe);
            await _appDbContext.SaveChangesAsync();
            return recipe;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateRecipeDTO updatedRecipe)
        {
            var existingRecipe = await _appDbContext.Recipes.FindAsync(id);

            if (existingRecipe != null)
            {
                return false;
            }

            existingRecipe.Name = updatedRecipe.Name;
            existingRecipe.Description = updatedRecipe.Description;
            existingRecipe.Ingredients = updatedRecipe.Ingredients;
            existingRecipe.Instructions = updatedRecipe.Instructions;

            _appDbContext.Entry(existingRecipe).State = EntityState.Modified;
            
            try
            {
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var recipeToRemove = await _appDbContext.Recipes.FindAsync(id);

            if (recipeToRemove == null)
            {
                return false;
            }

            _appDbContext.Recipes.Remove(recipeToRemove);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
