using Microsoft.EntityFrameworkCore;
using The_Gram.Data;
using The_Gram.Data.Models;
using The_Gram.Models.Post;
using The_Gram.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace The_Gram.Servicest
{
    public class PostService : IPostService
    {
       private readonly ApplicationDbContext context;
        private readonly IUserService userService;
        public PostService(ApplicationDbContext _context, IUserService _userService)
        {
            this.context = _context;
            this.userService = _userService;
        }
        public async Task<bool> CreatePost(PostCreationViewModel model, string[] imageUrls, UserProfile user)
        {
            bool output = false;
            if (user != null) 
            {
                if (model != null)
                {
                    Post post = new Post()
                    {
                        Text = model.Description,
                        UserId = user.Id,

                    };
                    if (imageUrls.Length > 0)
                    {
                        foreach (var imageUrl in imageUrls)
                        {
                            Image image = new Image()
                            {
                                URL = imageUrl,
                                ContentId = post.Id
                            };
                       await  context.Images.AddAsync(image);
                        }
                        await context.Posts.AddAsync(post);
                        await context.SaveChangesAsync();
                        output = true;
                    }
                    
                }
               
                

            }
            return output;
        }

        public async Task<List<AllPostsViewModel>> getAllAsync()
        {
            var allPostsViewModels = new List<AllPostsViewModel>();
           var posts = await context.Posts.ToListAsync();
            foreach (var post in posts)
            {
                var profile = await userService.GetProfileByIdAsync(post.UserId);
                var images =  await context.Images.Where(i => i.ContentId == post.Id).ToListAsync();
                var postViewModel = new AllPostsViewModel()
                {
                    Description = post.Text,
                    Images= images,
                    Id= post.Id,
                    Author = profile.Username,
                };
                allPostsViewModels.Add(postViewModel);
            }
            return allPostsViewModels;
        }

       public async Task<PostViewModel> Details(string id)
        {
             Post post = await GetByIdAsync(id);
            List<Image> images = await GetPostImages(post);
            List<PostReaction> likes = await GetPostLikes(post);
            List<PostComment> postComments = await GetPostComments(post);
            var profile = await userService.GetProfileByIdAsync(post.UserId);
            PostViewModel model = new PostViewModel()
            {
                Id = post.Id,
                AuthorId = post.UserId,
                Images= images,
                PostCaption = post.Text,
                PostId= post.Id,
                PostComments = postComments,
                Likes = likes,
                Author= profile.Username,
            };
            return model;
        }

        private async Task<List<PostComment>> GetPostComments(Post post)
        {
            var comments = await context.PostComments.Where(i => i.PostId == post.Id).ToListAsync();
            return comments;
        }
        private async Task<List<PostCommentReaction>> GetPostCommentReactions(Post post)
        {
            var allCommentReactions = new List<PostCommentReaction>();
            var comments = await context.PostComments.Where(i => i.PostId == post.Id).ToListAsync();
            foreach (var comment in comments)
            {
                var commentReactions = await context.PostCommentReactions.Where(i => i.CommentId == comment.Id).ToListAsync();
                foreach (var commentReaction in commentReactions)
                {
                    allCommentReactions.Add(commentReaction);
                }
            }
            return allCommentReactions ;
        }

        public async Task<List<Image>> GetPostImages(Post post)
        {
            var images = await context.Images.Where(i => i.ContentId == post.Id).ToListAsync();
            return images;
        }

        public async Task<Post> GetByIdAsync(string id)
        {
            return await context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> Like(Post post, UserProfile profile)
        {
            var liked = context.PostReactions.Where(pr => pr.UserId == profile.Id && pr.PostId == post.Id);
            if (liked != null || post == null || profile == null)
            {
                return false;
            }
            
            var reaction = new PostReaction()
            {
                PostId = post.Id,
                User = profile,
                UserId = profile.Id
            };
            post.TotalLikes++;

            post.Reactions.Add(reaction);
            await context.PostReactions.AddAsync(reaction);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<List<PostReaction>> GetPostLikes(Post post)
        {
            var likes = await context.PostReactions.Where(i => i.PostId == post.Id).ToListAsync();
            return likes;
        }

        public async Task<List<PostComment>> getPostComments(Post post)
        {
            if ( post == null)
            {
                return null;
            }
            var comments = await context.PostComments.Where(i => i.PostId == post.Id).ToListAsync();
            return comments;
        }

        public async Task<bool> Comment(Post post, UserProfile user, string commentText)
        {
            if ( post == null || user == null || commentText == string.Empty)
            {
                return false;
            }
            var postComment = new PostComment()
            {
                CommenterId= user.Id,
                PostId = post.Id,
                Content= commentText,
            };
            post.Comments.Add(postComment);
            await context.PostComments.AddAsync(postComment);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
