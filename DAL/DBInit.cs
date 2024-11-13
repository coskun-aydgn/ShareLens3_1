using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShareLens3.Models;

namespace ShareLens3.DAL
{
    public static class DBInit
    {
        public static async Task Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<PostDbContext>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            context.Database.EnsureCreated();

            if (!context.Posts.Any())
            {
                var posts = new List<Post>
                {
                    new Post
                    {
                        Title = "Pizza",
                        LikeCount = 150,
                        Description = "Delicious Italian dish with a thin crust topped with tomato sauce, cheese, and various toppings.",
                        ImageUrl = "/images/img1.webp",
                        CreatedDate = "24.12.2022"
                    },
                    // Additional posts...
                };
                context.AddRange(posts);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var users = new List<IdentityUser>
                {
                    new IdentityUser { UserName = "Alice_Hansen", Email = "Alice_Hansen@gmail.com" },
                    new IdentityUser { UserName = "Bob_Johansen", Email = "Bob_Johansen@gmail.com" }
                };

                foreach (var user in users)
                {
                    // Adding users and setting their password
                    await userManager.CreateAsync(user, "DefaultPassword123!"); // Set a default password here
                }
            }

            if (!context.Comments.Any())
            {
                var comments = new List<Comment>
                {
                    new Comment { Text = "Nice post!", CommentDate = "09.08.2023", PostId = 1 },
                    new Comment { Text = "Interesting!", CommentDate = "09.07.2023", PostId = 1 }
                };
                context.AddRange(comments);
                context.SaveChanges();
            }

            if (!context.UserPosts.Any())
            {
                var userPosts = new List<UserPost>
                {
                    new UserPost { UserId = "1", PostId = 2 },
                    new UserPost { UserId = "2", PostId = 2 },
                    new UserPost { UserId = "1", PostId = 4 }
                };
                context.AddRange(userPosts);
                context.SaveChanges();
            }

            if (!context.UserComments.Any())
            {
                var userComments = new List<UserComment>
                {
                    new UserComment { UserId = "1", CommentId = 2 },
                    new UserComment { UserId = "2", CommentId = 2 },
                    new UserComment { UserId = "1", CommentId = 1 }
                };
                context.AddRange(userComments);
                context.SaveChanges();
            }
        }
    }
}
