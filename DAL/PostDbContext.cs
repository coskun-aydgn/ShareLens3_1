using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShareLens3.Models;

namespace ShareLens3.DAL;
public class PostDbContext : IdentityDbContext<User>
{
	public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
	{
       // Database.EnsureCreated();
	}

	public DbSet<Post> Posts { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<User> Users { get; set; }
	public  DbSet<UserPost> UserPosts { get; set; }
    public  DbSet<UserComment> UserComments { get; set; } 
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseLazyLoadingProxies();
	}
}