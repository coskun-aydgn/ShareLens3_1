
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShareLens3.DAL;

public class ApplicationDbInitializer
{
    public static async Task InitializeAsync(
        PostDbContext db,
        UserManager<IdentityUser> um,
        RoleManager<IdentityRole> rm)
    {
        try
        {
            // Ensure database is created
            if (db.Database.IsSqlite())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            // Set solar panel prices
           
            // Save changes
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log exception
            Console.WriteLine($"An error occurred during database initialization: {ex.Message}");
        }
    }
}