using Microsoft.AspNetCore.Mvc;
using ShareLens3.DAL;
using ShareLens3.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ShareLens3.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _db;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository db, ILogger<UserController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Display all users
        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _db.GetAllUsers();
                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user list");
                return View("Error");
            }
        }

        // Display details of a specific user
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var user = await _db.GetUserById(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    return NotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user details for ID {UserId}", id);
                return View("Error");
            }
        }

        // Display form to create a new user
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Add a new user
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                var success = await _db.AddUser(user);
                if (success)
                {
                    await _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "User could not be added");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user");
                return View("Error");
            }
        }

        // Display form to edit a user
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var user = await _db.GetUserById(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    return NotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user details for edit with ID {UserId}", id);
                return View("Error");
            }
        }

        // Update user details
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                var success = await _db.UpdateUser(user);
                if (success)
                {
                    await _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "User could not be updated");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}", user.UserId);
                return View("Error");
            }
        }

        // Display form to confirm deletion of a user
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _db.GetUserById(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    return NotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user details for delete with ID {UserId}", id);
                return View("Error");
            }
        }

        // Delete a user
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _db.DeleteUser(id);
                if (success)
                {
                    await _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogWarning("Failed to delete user with ID {UserId}", id);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
                return View("Error");
            }
        }
    }
}
