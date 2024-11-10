using ShareLens3.Models;

namespace ShareLens3.DAL
{
    public interface ICommentRepository
    {
        Task<Post?> GetPostById(int postId);
        Task AddLike(int postId);
        Task AddComment(int postId, string text);
    }
}
