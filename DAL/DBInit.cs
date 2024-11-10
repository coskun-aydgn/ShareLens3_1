using Microsoft.EntityFrameworkCore;
using ShareLens3.Models;

namespace ShareLens3.DAL;
public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        PostDbContext context = serviceScope.ServiceProvider.GetRequiredService<PostDbContext>();
       //
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
                    CreatedDate="24.12.2022"
                },
                new Post
                {
                    Title = "Fried Chicken Leg",
                    LikeCount = 20,
                    Description = "Crispy and succulent chicken leg that is deep-fried to perfection, often served as a popular fast food item.",
                    ImageUrl = "/images/img2.webp",
                    CreatedDate="18.12.2022"
                },
                new Post
                {
                    Title = "French Fries",
                    LikeCount = 50,
                    Description = "Crispy, golden-brown potato slices seasoned with salt and often served as a popular side dish or snack.",
                    ImageUrl = "/images/img3.webp",
                    CreatedDate="18.06.2022"
                },
                new Post
                {
                    Title = "Grilled Ribs",
                    LikeCount = 250,
                    Description = "Tender and flavorful ribs grilled to perfection, usually served with barbecue sauce.",
                    ImageUrl = "/images/img4.webp",
                    CreatedDate="18.06.2023"
                },
                new Post
                {
                    Title = "Tacos",
                    LikeCount = 150,
                    Description = "Tortillas filled with various ingredients such as seasoned meat, vegetables, and salsa, folded into a delicious handheld meal.",
                    ImageUrl = "/images/img5.webp",
                    CreatedDate="18.08.2023"
                },
                new Post
                {
                    Title = "Fish and Chips",
                    LikeCount = 180,
                    Description = "Classic British dish featuring battered and deep-fried fish served with thick-cut fried potatoes.",
                    ImageUrl = "/images/img6.webp",
                    CreatedDate="19.08.2023"
                },
                new Post
                {
                    Title = "Cider",
                    LikeCount = 50,
                    Description = "Refreshing alcoholic beverage made from fermented apple juice, available in various flavors.",
                    ImageUrl = "/images/img7.webp",
                    CreatedDate="19.01.2023"
                },
                new Post
                {
                    Title = "Coke",
                    LikeCount = 30,
                    Description = "Popular carbonated soft drink known for its sweet and refreshing taste.",
                    ImageUrl = "/images/img8.webp",
                    CreatedDate="19.01.2024"
                },
            };
            context.AddRange(posts);
            context.SaveChanges();
        }

        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User { Name = "Alice Hansen", UserName = "Alice_Hansen", 
                                Email="Alice_Hansen@gmail.com", Password="1234rfv" },
                new User { Name = "Bob Johansen", UserName = "Bob_Johansen",
                            Email="Bob_Johansen@gmail.com", Password="3456yhn"},
            };
            context.AddRange(users);
            context.SaveChanges();
        }

        if (!context.Comments.Any())
        {
            var comments = new List<Comment>
            {
                new Comment {Text="asjdgfuasgdfoudguygdsafyufgasuyf",
                 CommentDate = "09.08.2023", PostId=1},
                new Comment {Text="asjdgfuasgdfoudguygdsafyufgasuyf",
                 CommentDate = "09.07.2023", PostId=1},
            };
            context.AddRange(comments);
            context.SaveChanges();
        }

        if (!context.UserPosts.Any())
        {
            var userPosts = new List<UserPost>
            {
                new UserPost { UserId = 1, PostId = 2},
                new UserPost { UserId = 2, PostId = 2,},
                new UserPost { UserId = 1, PostId = 4,},
            };
            context.AddRange(userPosts);
            context.SaveChanges();
        }
        if (!context.UserComments.Any())
        {
            var userComments = new List<UserComment>
            {
                new UserComment { UserId = 1, CommentId = 2},
                new UserComment { UserId = 2, CommentId = 2,},
                new UserComment { UserId = 1, CommentId = 1,},
            };
            context.AddRange(userComments);
            context.SaveChanges();
        }
        
    }
}