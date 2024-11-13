using Microsoft.AspNetCore.Identity;

namespace ShareLens3.Models;

public class UserComment
{
    public int UserCommentId { get; set; }

    public string UserId { get; set; } = default!; // Assuming UserId is a string with IdentityUser
    public virtual IdentityUser User { get; set; } = default!; // Set the type to IdentityUser if using Identity

    public int CommentId { get; set; }
    public virtual Comment Comment { get; set; } = default!;
}
