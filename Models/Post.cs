using System;
using System.ComponentModel.DataAnnotations;

namespace ShareLens3.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; } // Yüklenen dosyanın yolunu saklar

        public string? CreatedDate { get; set; }

        public int LikeCount { get; set; }
        public int? UserId { get; set; }  // Post'u oluşturan kullanıcının Id'si
        public virtual User? User { get; set; } = default!;  // İlişkiyi kurmak için
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        public virtual List<UserPost> UserPosts { get; set; } = new List<UserPost>();
        
    }
}
