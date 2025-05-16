using backend.DTOs;

namespace backend.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentResponseDTO>> GetCommentsForRecipeAsync(Guid recipeId);
        Task<CommentResponseDTO?> AddCommentAsync(Guid recipeId, string userId, CreateCommentDTO createDto);
        Task<bool> DeleteCommentAsync(Guid commentId, string userId);
    }
}
