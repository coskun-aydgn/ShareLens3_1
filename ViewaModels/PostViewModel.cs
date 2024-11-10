using ShareLens3.Models;

namespace ShareLens3.ViewModels
{
    public class PostViewModel
    {
        public IEnumerable<Post> Posts;
        public string? CurrentViewName;
        public PostViewModel(IEnumerable<Post> posts, string? currentViewName)
        {
            Posts=posts;
            CurrentViewName=currentViewName;
        }
    }
}