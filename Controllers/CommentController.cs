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
            ViewData["Users"] = await _commentRepository.GetAllUsers(); // Kullanıcıları ViewData ile gönderiyoruz

            return View("/Views/Post/AddComment.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, int userId, string text)
        {
            if (postId == 0 || userId == 0 || string.IsNullOrWhiteSpace(text))
            {
                ModelState.AddModelError("", "All fields are required.");
                ViewData["Users"] = await _commentRepository.GetAllUsers();
                return View();
            }

            await _commentRepository.AddComment(postId, userId, text);
            return RedirectToAction("Details", "Post", new { id = postId });
        }
    }
}
