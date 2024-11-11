using Microsoft.EntityFrameworkCore;
using ShareLens3.Models;


namespace ShareLens3.DAL
{
    public class CommentRepository : ICommentRepository
    {
        private readonly PostDbContext _db;
        private readonly ILogger<PostRepository> _logger;

        public CommentRepository(PostDbContext db, ILogger<PostRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Post?> GetPostById(int postId)
        {
            return await _db.Posts
                .Include(p => p.Comments)
                .ThenInclude(c => c.UserComments)
                .ThenInclude(uc => uc.User)
                .FirstOrDefaultAsync(p => p.PostId == postId);
        }

        public async Task<bool> AddComment(int postId, int userId, string text)
        {
            try
            {
                var newComment = new Comment
                {
                    Text = text,
                    CommentDate = DateTime.Now.ToString("dd.MM.yyyy"),
                    PostId = postId,
                    UserComments = new List<UserComment>
                {
                    new UserComment { UserId = userId }
                }
                };

                _db.Comments.Add(newComment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {

                _logger.LogError("[CommentRepository] items Add() failed when AddComment(), error message:{e}", e.Message);
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                return await _db.Users.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] posts ToListAsync() failed when GetAllUsers(), error message:{e}", e.Message);
                return null;
            }
            
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
               _logger.LogError("[CommentRepository] posts SaveChangesAsync() failed when SaveChanges(), error message:{e}", e.Message);
                return false;
            }
            
        }
    }
}
