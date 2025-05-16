using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public interface IRecipeService
    {
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<RecipeResponseDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<Recipe>> GetUserRecipesAsync(string userId);
        Task<Recipe> CreateAsync(CreateRecipeDTO newRecipe, string userId);
        Task<bool> UpdateAsync(Guid id, UpdateRecipeDTO updatedRecipe);
        Task<bool> DeleteAsync(Guid id);

    }
}
