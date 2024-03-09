using The_Gram.Data.Models;
using The_Gram.Models.Post;

namespace The_Gram.Services
{
    public interface IPostService
    {
        public Task<bool> CreatePost(PostCreationViewModel model, string[] imageUrls, UserProfile user);
        public Task<PostViewModel> Details(string id);
        public Task<List<AllPostsViewModel>> getAllAsync();
        public Task<Post> GetByIdAsync(string postId);
        public Task<List<Image>> GetPostImages(Post post);
        public Task<bool> Like(Post post, UserProfile profile);
        public Task<List<PostReaction>> GetPostLikes(Post post);
        public Task<List<PostComment>> getPostComments(Post post);
        public Task<bool> Comment(Post post, UserProfile user, string commentText);
    }
}