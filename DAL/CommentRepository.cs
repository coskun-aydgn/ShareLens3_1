using Microsoft.EntityFrameworkCore;
using ShareLens3.Models;

namespace ShareLens3.DAL
{
    public class CommentRepository : ICommentRepository
    {
        private readonly PostDbContext _db;

        public CommentRepository(PostDbContext db)
        {
            _db = db;
        }

        public async Task<Post?> GetPostById(int postId)
        {
            return await _db.Posts
                .Include(p => p.Comments)
                .ThenInclude(c => c.UserComments)
                .ThenInclude(uc => uc.User)
                .FirstOrDefaultAsync(p => p.PostId == postId);
        }

        public async Task AddComment(int postId, int userId, string text)
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
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
