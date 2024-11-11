using ShareLens3.Models;

namespace ShareLens3.DAL
{
    public interface ICommentRepository
    {
        Task<Post?> GetPostById(int postId); // Post'u yorumlarıyla birlikte getirir
        Task AddComment(int postId, int userId, string text); // Yorum ekler
        Task<IEnumerable<User>> GetAllUsers(); // Kullanıcıları getirir
        Task SaveChanges(); // Veritabanı değişikliklerini kaydeder
    }
}
