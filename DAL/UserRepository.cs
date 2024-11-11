using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using ShareLens3.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace ShareLens3.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly PostDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(PostDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Getting all users with associated posts and comments.");
                return await _context.Users
                    .Include(u => u.UserPosts)
                    .Include(u => u.UserComments)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users.");
                throw;
            }
        }

        public async Task<User?> GetUserById(int userId)
        {
            try
            {
                _logger.LogInformation("Getting user by ID: {UserId}", userId);
                return await _context.Users
                    .Include(u => u.UserPosts)
                    .Include(u => u.UserComments)
                    .FirstOrDefaultAsync(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the user by ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                _logger.LogInformation("Getting user by email: {Email}", email);
                return await _context.Users
                    .Include(u => u.UserPosts)
                    .Include(u => u.UserComments)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the user by email: {Email}", email);
                throw;
            }
        }

        public async Task<bool> AddUser(User user)
        {
            try
            {
                _logger.LogInformation("Adding new user: {UserName}", user.UserName);
                await _context.Users.AddAsync(user);
                await SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new user: {UserName}", user.UserName);
                return false;
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                _logger.LogInformation("Updating user: {UserId}", user.UserId);
                _context.Users.Update(user);
                await SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user: {UserId}", user.UserId);
                return false;
            }
        }

        public async Task<bool> DeleteUser(int userId)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID: {UserId}", userId);
                var user = await GetUserById(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await SaveChanges();
                    return true;
                }
                else
                {
                    _logger.LogWarning("User with ID {UserId} not found.", userId);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user with ID: {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
                _logger.LogInformation("Saving changes to the database.");
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes to the database.");
                return false;
            }
        }
    }
}
