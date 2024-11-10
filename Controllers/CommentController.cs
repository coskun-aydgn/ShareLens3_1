using Microsoft.AspNetCore.Mvc;
using ShareLens3.DAL;
using ShareLens3.Models;

namespace ShareLens3.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddLike(int postId, string currentPage)
        {
            await _commentRepository.AddLike(postId);

            return currentPage switch
            {
                "PostTable" => RedirectToAction("PostTable", "Post"),
                "PostGrid" => RedirectToAction("PostGrid", "Post"),
                "Details" => RedirectToAction("Details", "Post", new { id = postId }),
                _ => RedirectToAction("PostTable", "Post")
            };
        }

        [HttpGet]
        public async Task<IActionResult> AddComment(int id)
        {
            var post = await _commentRepository.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string text)
        {
            await _commentRepository.AddComment(postId, text);
            return RedirectToAction("Details", "Post", new { id = postId });
        }
    }
}
