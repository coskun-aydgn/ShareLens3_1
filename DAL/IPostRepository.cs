using ShareLens3.Models;

namespace ShareLens3.DAL;

public interface IPostRepository
{
    Task<IEnumerable<Post>?> GetAll();
    Task<Post?> GetPostById(int id);
    Task<bool> Create(Post post);
    Task<bool> Update(Post post);
    Task<bool> Delete(int id);
    Task<bool> AddLike(int postId);
    Task<User?> GetRandomUser(); // Yeni metod
    Task<Comment?> GetRandomComment(); // Yeni metod
}