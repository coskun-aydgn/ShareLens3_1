using Microsoft.EntityFrameworkCore;
using ShareLens3.DAL;
using ShareLens3.Models;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PostDbContextConnection") ?? throw new InvalidOperationException("Connection string 'PostDbContextConnection' not found.");

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });;

builder.Services.AddDbContext<PostDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:PostDbContextConnection"]);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<PostDbContext>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
var loggerConfiguration=new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File($"Logs/app_{DateTime.Now:yyMMdd_HHmmss},log");

loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                            e.Level == LogEventLevel.Information &&
                            e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger=loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();
/*using (var service = app.Services.CreateScope())
{
    var db = service.ServiceProvider.GetRequiredService<PostDbContext>();
    var um = service.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var rm = service.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    ApplicationDbInitializer.InitializeAsync(db,um,rm);
}*/

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
   // DBInit.Seed(app);
}

    

app.UseStaticFiles();
app.UseAuthentication();

//app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

