using ShareLens3.Models;

namespace ShareLens3.DAL;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAll();
    Task<Post?> GetPostById(int id);
    Task Create(Post post);
    Task Update(Post post);
    Task<bool> Delete(int id);

    Task<User?> GetRandomUser(); // Yeni metod
    Task<Comment?> GetRandomComment(); // Yeni metod
}