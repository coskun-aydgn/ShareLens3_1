using Microsoft.EntityFrameworkCore;
using ShareLens3.Models;

namespace ShareLens3.DAL;

public class PostRepository : IPostRepository
{
    private readonly PostDbContext _db;

    public PostRepository(PostDbContext db)
    {
        _db = db;
    }
    public async Task<IEnumerable<Post>> GetAll()
    {
        return await _db.Posts.ToListAsync();
    }
    public async Task<Post?> GetPostById(int id)
    {
            return await _db.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.PostId == id);

    }
    public async Task Create(Post post)
    {
        _db.Posts.Add(post);
        await _db.SaveChangesAsync();
    }
    public async Task Update(Post post)
    {
        _db.Posts.Update(post);
        await _db.SaveChangesAsync();
    }
    public async Task<bool> Delete(int id)
    {
        var post = await _db.Posts.FindAsync(id);
        if (post == null)
        {
            return false;
        }
        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<User> GetRandomUser()
    {
        // Veritabanından kullanıcıları al ve bellekte sırala
        return (await _db.Users.ToListAsync())
            .OrderBy(u => Guid.NewGuid())
            .FirstOrDefault();
    }

    public async Task<Comment> GetRandomComment()
    {
        // Veritabanından yorumları al ve bellekte sırala
        return (await _db.Comments.ToListAsync())
            .OrderBy(c => Guid.NewGuid())
            .FirstOrDefault();
    }

}