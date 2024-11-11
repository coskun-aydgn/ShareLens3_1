using Microsoft.EntityFrameworkCore;
using ShareLens3.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });;

builder.Services.AddDbContext<PostDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:PostDbContextConnection"]);
});

builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

    

app.UseStaticFiles();

app.MapDefaultControllerRoute();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

