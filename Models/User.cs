

namespace ShareLens3.Models
{
    public class User
    {
        public int UserId { get; set; }

      
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;

        public virtual List<UserPost>? UserPosts { get; set; }
        public virtual List<UserComment>? UserComments { get; set; } 
    }
}
