using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ShareLens3.Models;

namespace ShareLens3.DAL
{
    public class CommentRepository : ICommentRepository
    {
        private readonly PostDbContext _db;
        private readonly ILogger<CommentRepository> _logger;

        public CommentRepository(PostDbContext db, ILogger<CommentRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Post?> GetPostById(int postId)
        {
            return await _db.Posts
                .Include(p => p.Comments)
                .ThenInclude(c => c.UserComments)
                .ThenInclude(uc => uc.User) // Ensure this is compatible with IdentityUser
                .FirstOrDefaultAsync(p => p.PostId == postId);
        }

        public async Task<bool> AddComment(int postId, string userId, string text)
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
                        new UserComment { UserId = userId } // Use string type for IdentityUser Id
                    }
                };

                _db.Comments.Add(newComment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] AddComment() failed with error: {Error}", e.Message);
                return false;
            }
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            try
            {
                return await _db.Users.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] GetAllUsers() failed with error: {Error}", e.Message);
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
                _logger.LogError("[CommentRepository] SaveChanges() failed with error: {Error}", e.Message);
                return false;
            }
        }
    }
}
