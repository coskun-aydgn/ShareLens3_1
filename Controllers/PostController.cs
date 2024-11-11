using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareLens3.Models;
using ShareLens3.ViewModels;
using ShareLens3.DAL;
using System;

namespace ShareLens3.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostRepository postRepository, ILogger<PostController> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<IActionResult> PostTable()
        {
            try
            {
                var posts = await _postRepository.GetAll();
                var postsViewModel = new PostViewModel(posts, "PostTable");
                return View(postsViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving posts for PostTable view.");
                return View("Error");
            }
        }

        public async Task<IActionResult> PostGrid()
        {
            try
            {
                var posts = await _postRepository.GetAll();
                var postsViewModel = new PostViewModel(posts, "PostGrid");
                return View(postsViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving posts for PostGrid view.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var post = await _postRepository.GetPostById(id);
                if (post == null)
                {
                    _logger.LogWarning("Post with ID {PostId} not found.", id);
                    return NotFound();
                }
                return View(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving details for post with ID {PostId}.", id);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View(post);
            }

            try
            {
                post.CreatedDate = DateTime.Now.ToString("dd.MM.yyyy");
                post.LikeCount = 0;

                // Randomly assign a user and comment
                var randomUser = await _postRepository.GetRandomUser();
                if (randomUser != null)
                {
                    post.UserPosts = new List<UserPost> { new UserPost { UserId = randomUser.UserId } };
                }

                var randomComment = await _postRepository.GetRandomComment();
                if (randomComment != null)
                {
                    post.Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Text = randomComment.Text,
                            CommentDate = DateTime.Now.ToString("dd.MM.yyyy"),
                            UserComments = new List<UserComment> { new UserComment { UserId = randomUser?.UserId ?? 1 } }
                        }
                    };
                }

                // Image upload handling
                var postImage = Request.Form.Files.FirstOrDefault();
                if (postImage != null && postImage.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    var uniqueFileName = Guid.NewGuid() + "_" + postImage.FileName;
                    var filePath = Path.Combine(uploads, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        postImage.CopyTo(fileStream);
                    }

                    post.ImageUrl = "/images/" + uniqueFileName;
                }

                await _postRepository.Create(post);
                return RedirectToAction(nameof(PostTable));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating a new post.");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var post = await _postRepository.GetPostById(id);
                if (post == null)
                {
                    _logger.LogWarning("Post with ID {PostId} not found for updating.", id);
                    return NotFound();
                }
                return View(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving post for update with ID {PostId}.", id);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View(post);
            }

            try
            {
                var existingPost = await _postRepository.GetPostById(post.PostId);
                if (existingPost == null)
                {
                    _logger.LogWarning("Post with ID {PostId} not found for updating.", post.PostId);
                    return NotFound();
                }

                // Preserve existing fields
                post.CreatedDate = existingPost.CreatedDate;
                post.LikeCount = existingPost.LikeCount;

                // Image upload handling
                var postImage = Request.Form.Files.FirstOrDefault();
                if (postImage != null && postImage.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    var uniqueFileName = Guid.NewGuid() + "_" + postImage.FileName;
                    var filePath = Path.Combine(uploads, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        postImage.CopyTo(fileStream);
                    }

                    post.ImageUrl = "/images/" + uniqueFileName;
                }
                else
                {
                    post.ImageUrl = existingPost.ImageUrl;
                }

                await _postRepository.Update(post);
                return RedirectToAction(nameof(PostTable));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating post with ID {PostId}.", post.PostId);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var post = await _postRepository.GetPostById(id);
                if (post == null)
                {
                    _logger.LogWarning("Post with ID {PostId} not found for deletion.", id);
                    return NotFound();
                }
                return View(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving post for deletion with ID {PostId}.", id);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var post = await _postRepository.GetPostById(id);
                if (post == null)
                {
                    _logger.LogWarning("Post with ID {PostId} not found for deletion.", id);
                    return NotFound();
                }

                await _postRepository.Delete(id);
                return RedirectToAction(nameof(PostTable));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting post with ID {PostId}.", id);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddLike(int postId, string currentPage)
        {
            try
            {
                await _postRepository.AddLike(postId);
                _logger.LogInformation("Like added to post with ID {PostId}.", postId);

                return currentPage switch
                {
                    "PostTable" => RedirectToAction("PostTable"),
                    "PostGrid" => RedirectToAction("PostGrid"),
                    "Details" => RedirectToAction("Details", new { id = postId }),
                    _ => RedirectToAction("PostTable")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding like to post with ID {PostId}.", postId);
                return View("Error");
            }
        }
    }
}
