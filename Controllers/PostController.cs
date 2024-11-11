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

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IActionResult> PostTable()
        {
            var posts = await _postRepository.GetAll();
            var postsViewModel = new PostViewModel(posts, "PostTable");
            return View(postsViewModel);
        }

        public async Task<IActionResult> PostGrid()
        {
            var posts = await _postRepository.GetAll();
            var postsViewModel = new PostViewModel(posts, "PostGrid");
            return View(postsViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _postRepository.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
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
                return View(post); // Hatalı model ile formu yeniden göster
            }

            post.CreatedDate = DateTime.Now.ToString("dd.MM.yyyy");
            post.LikeCount = 0;

            // Kullanıcı ve yorumları rastgele atama
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

            // Resim yükleme
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

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var post = await _postRepository.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View(post);
            }

            // Mevcut post'u izlemeye almadan çekiyoruz
            var existingPost = await _postRepository.GetPostById(post.PostId);
            if (existingPost == null)
            {
                return NotFound();
            }

            // Post'un `CreatedDate` ve `LikeCount` alanlarını koruyoruz
            post.CreatedDate = existingPost.CreatedDate;
            post.LikeCount = existingPost.LikeCount;

            // Dosya işlemleri
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

            // Post nesnesini izlemeye alarak güncelle
            await _postRepository.Update(post);

            return RedirectToAction(nameof(PostTable));
        }




        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postRepository.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _postRepository.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            await _postRepository.Delete(id);
            return RedirectToAction(nameof(PostTable));
        }
        [HttpPost]
        public async Task<IActionResult> AddLike(int postId, string currentPage)
        {
            await _postRepository.AddLike(postId);

            // Kullanıcının geldiği sayfaya yönlendir
            return currentPage switch
            {
                "PostTable" => RedirectToAction("PostTable"),
                "PostGrid" => RedirectToAction("PostGrid"),
                "Details" => RedirectToAction("Details", new { id = postId }),
                _ => RedirectToAction("PostTable")
            };
        }




    }
}
