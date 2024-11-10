namespace ShareLens3.Models;


public class UserComment
{
    public int UserCommentId { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; } = default!;

    public int CommentId { get; set; }
    public virtual Comment Comment { get; set; } = default!;
}
