
namespace ShareLens3.Models
{
    public class Comment
    {
        public int CommentId { get; set; }


        public string Text { get; set; } = string.Empty;

        public string? CommentDate { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; } = default!;

        public virtual List<UserComment>? UserComments { get; set; }
    }
}