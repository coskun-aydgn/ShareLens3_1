using Microsoft.AspNetCore.Mvc;
using ShareLens3.DAL;
using ShareLens3.Models;
using System.Threading.Tasks;

namespace ShareLens3.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> AddComment(int id)
        {
            var post = await _commentRepository.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            ViewData["PostId"] = id;
            ViewData["Users"] = await _commentRepository.GetAllUsers(); // Passing user list to view

            return View("/Views/Post/AddComment.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string userId, string text)
        {
            if (postId == 0 || string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(text))
            {
                ModelState.AddModelError("", "All fields are required.");
                ViewData["Users"] = await _commentRepository.GetAllUsers();
                return View("/Views/Post/AddComment.cshtml"); // Return to the correct view if validation fails
            }

            await _commentRepository.AddComment(postId, userId, text);
            return RedirectToAction("Details", "Post", new { id = postId });
        }
    }
}
