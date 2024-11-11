using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShareLens3.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ.\- ]{2,20}", ErrorMessage = "The Title must be alphanumeric and between 2 to 20 characters.")]
       
        public string Title { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Description cannot exceed 300 characters.")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? ImageUrl { get; set; }  // Stores the path to the uploaded file

        public string? CreatedDate { get; set; }

        public int LikeCount { get; set; }

        public int? UserId { get; set; }  // ID of the user who created the post

        public virtual User? User { get; set; } = default!;  // To establish the relationship with the User entity

        public virtual List<Comment> Comments { get; set; } = new List<Comment>();

        public virtual List<UserPost> UserPosts { get; set; } = new List<UserPost>();
    }
}
