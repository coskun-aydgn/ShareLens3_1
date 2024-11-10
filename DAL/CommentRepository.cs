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
            return await _db.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.PostId == postId);
        }

        public async Task AddLike(int postId)
        {
            var post = await _db.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
            if (post != null)
            {
                post.LikeCount++;
                await _db.SaveChangesAsync();
            }
        }

        public async Task AddComment(int postId, string text)
        {
            var post = await GetPostById(postId);
            if (post == null) return;

            var randomUser = _db.Users
                .AsEnumerable()
                .OrderBy(u => Guid.NewGuid())
                .FirstOrDefault();

            if (randomUser == null) return;

            var newComment = new Comment
            {
                Text = text,
                CommentDate = DateTime.Now.ToString("dd.MM.yyyy"),
                PostId = postId,
                UserComments = new List<UserComment> { new UserComment { UserId = randomUser.UserId } }
            };

            _db.Comments.Add(newComment);
            await _db.SaveChangesAsync();
        }
    }
}
