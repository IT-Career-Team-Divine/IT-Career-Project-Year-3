using The_Gram.Data.Models;
using The_Gram.Data.Models.The_Gram.Data.Models;
using The_Gram.Models.Post;

namespace The_Gram.Services
{
    public interface IPostService
    {
        public Task<bool> CreatePost(PostCreationViewModel model, string[] imageUrls, UserProfile user);
        public Task<PostViewModel> Details(string id);
        public Task<List<AllPostsViewModel>> getAllAsync();
        public Task<Post> GetByIdAsync(string postId);
        public Task<List<Image>> GetPostImages(string postId);
        public Task<bool> Like(Post post, UserProfile profile);
        public Task<bool> Dislike(Post post, UserProfile profile);
        public Task<List<PostReaction>> GetPostLikes(string postId);
        public Task<List<PostReaction>> GetPostDislikes(string postId);
        public Task<List<AllPostsViewModel>> GetUserPosts(string id);
        public Task<List<PostComment>> getPostComments(string postId);
        public Task<bool> Comment(Post post, UserProfile user, string commentText);
        public  Task<List<AllPostsViewModel>> GetFeedAsync(string id);
        Task Edit(string id, List<Image> images, string postCaption);
    }
}