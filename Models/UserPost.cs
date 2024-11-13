using Microsoft.AspNetCore.Identity;

namespace ShareLens3.Models;

public class UserPost
{
    public int UserPostId { get; set; }
    
    public string UserId { get; set; } = default!; // Assuming UserId is of type string with IdentityUser
    public virtual IdentityUser User { get; set; } = default!; // Set the type to IdentityUser if using Identity

    public int PostId { get; set; }
    public virtual Post Post { get; set; } = default!;
}
