using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareLens3.Models;
using ShareLens3.DAL;


namespace ShareLens3.Controllers
{
    public class UserController : Controller
    {
        private readonly PostDbContext _postDbContext;
        public UserController(PostDbContext postDbContext)
        {
            _postDbContext = postDbContext;
        }
        public async Task<IActionResult> PostTable()
        {
            List<User> users = await _postDbContext.Users.ToListAsync();
            return View(users);
        }
    }
}