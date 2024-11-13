using Microsoft.AspNetCore.Identity;
using ShareLens3.Models;

namespace ShareLens3.DAL
{
    public interface ICommentRepository
    {
        Task<Post?> GetPostById(int postId); // Retrieves the post along with its comments
        Task<bool> AddComment(int postId, string userId, string text); // Adds a comment, adjusted userId type to string
        Task<IEnumerable<IdentityUser>> GetAllUsers(); // Retrieves users, updated to return IdentityUser
        Task<bool> SaveChanges(); // Saves changes to the database
    }
}
