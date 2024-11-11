using Microsoft.EntityFrameworkCore;
using ShareLens3.Models;

namespace ShareLens3.DAL;

public class PostRepository : IPostRepository
{
    private readonly PostDbContext _db;
    private readonly ILogger<PostRepository> _logger;

    public PostRepository(PostDbContext db, ILogger<PostRepository> logger)
    {
        _db = db;
        _logger=logger;
    }
    public async Task<IEnumerable<Post>?> GetAll()
    {
        try
        {
            return await _db.Posts.ToListAsync();
        }
        catch (Exception e)
        {
           _logger.LogError("[PostRepository] items ToListAsync() failed when GetAll(), error message:{e}", e.Message);
           return null;
        }
        
    }
    public async Task<Post?> GetPostById(int id)
    { 
        try
        {
             return await _db.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.PostId == id);
        }
        catch (Exception e)
        {
            _logger.LogError("[PostRepository] item FirstOrDefaultAsync(id) failed when GetItemById for PostId {PostId:0000}, error message: {e}", id, e.Message);
            return null;
        }

    }
    public async Task<bool> Create(Post post)
    {
        try
        {
        _db.Posts.Add(post);
        await _db.SaveChangesAsync();
        return true;
        }
         
        catch (Exception e)
        {
            _logger.LogError("[PostRepository] item creation failed for post {@post}, error message: {e}", post, e.Message);
            return false;
        }
    }
    public async Task<bool> Update(Post post)
    {
        try{
        _db.Posts.Update(post);
        await _db.SaveChangesAsync();
        return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[PostRepository] item FindAsync(id) failed when updating the PostId {PostId:0000}, error message: {e}", post, e.Message);
            return false;
        }
    }
    public async Task<bool> Delete(int id)
    {
        try{
        var post = await _db.Posts.FindAsync(id);
        if (post == null)
        {
            return false;
        }
        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();
        return true;
        }
         catch (Exception e)
        {
            _logger.LogError("[PostRepository] post deletion failed for the PostId {PostId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
    public async Task<User?> GetRandomUser()
    {
        // Veritabanından kullanıcıları al ve bellekte sırala
        return (await _db.Users.ToListAsync())
            .OrderBy(u => Guid.NewGuid())
            .FirstOrDefault();
    }

    public async Task<Comment?> GetRandomComment()
    {
        // Veritabanından yorumları al ve bellekte sırala
        return (await _db.Comments.ToListAsync())
            .OrderBy(c => Guid.NewGuid())
            .FirstOrDefault();
    }
     public async Task<bool> AddLike(int id)
    {
        try{
        var post = await _db.Posts.FindAsync(id);
        if (post != null)
        {
            post.LikeCount++;  // Like sayısını bir artır
            await _db.SaveChangesAsync();  // Veritabanında değişiklikleri kaydet
            return true;
        }else return false;
        }
        catch(Exception e)
        {
            _logger.LogError("[PostRepository] post Addliked failed for the PostId {PostId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }

}