using DynamicVML;
using DynamicVML.Internals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using The_Gram.Models.Post;
using The_Gram.Models.User;
using The_Gram.Data.Models;
using The_Gram.Services;
using Microsoft.AspNetCore.Identity;

namespace The_Gram.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService postService;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;
        public PostController(IPostService _postService, IUserService _userService, UserManager<User> _userManager)
        {
            postService= _postService;
            userService= _userService;
            userManager= _userManager;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Post()
        {
              var  model = new PostCreationViewModel();
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(PostCreationViewModel model, string[] imageUrls)
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var currentProfile = await userService.GetProfileByIdAsync(currentUser.CurrentProfileId);
            var result = await postService.CreatePost(model,imageUrls,currentProfile);
            if (!result)
            {
                return View(model);
            }
            return RedirectToAction("Index", "Home");

        }
        [Authorize]
        public async Task<IActionResult> All()
        {
            List<AllPostsViewModel> posts = await postService.getAllAsync();
            return View(posts);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            PostViewModel post = await postService.Details(id);
            return View(post);
        }
        [HttpGet]
        public async Task<IActionResult> PrevImage(string postId, int currentImageIndex)
        {
            int prevIndex = 0;
            var post = await postService.GetByIdAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            var images = await postService.GetPostImages(post);
            var likes = await postService.GetPostLikes(post);

            if (currentImageIndex > 0)
            {
                 prevIndex = currentImageIndex - 1;

            }
            else
            {
                prevIndex = 0;
            }

            var imageUrl =images[prevIndex].URL;

            return View("Details", new PostViewModel { PostCaption = post.Text, Images = images, Likes = likes, PostComments = post.Comments, Id = post.Id, CurrentImageIndex = prevIndex });
        }

        [HttpGet]
        public async Task<IActionResult> NextImage(string postId, int currentImageIndex)
        {
            int nextIndex = 0;
            var post = await postService.GetByIdAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            var images = await postService.GetPostImages(post);
            var likes = await postService.GetPostLikes(post);
            if (currentImageIndex != images.Count - 1)
            {
                nextIndex = currentImageIndex + 1;
            }

            var imageUrl = images[nextIndex].URL;

            return View("Details", new PostViewModel { PostCaption = post.Text, Images = images, Likes = likes, PostComments = post.Comments, Id = post.Id, CurrentImageIndex = nextIndex });
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Like(string postId, int currentImageIndex)
        {
            var post = await postService.GetByIdAsync(postId);
            var user = await userService.GetProfileByIdAsync(post.UserId);
            var images = await postService.GetPostImages(post);

            await postService.Like(post, user);

            var likes = await postService.GetPostLikes(post);

            return View("Details", new PostViewModel { PostCaption = post.Text, Images = images, Likes =likes, PostComments = post.Comments, Id = post.Id, CurrentImageIndex = currentImageIndex });


        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(string postId, int currentImageIndex, string commentText)
        {
            var post = await postService.GetByIdAsync(postId);
            var user = await userService.GetProfileByIdAsync(post.UserId);
            var images = await postService.GetPostImages(post);
            var likes = await postService.GetPostLikes(post);

            await postService.Comment(post, user, commentText);
            List<PostComment> comments = await postService.getPostComments(post);

            return View("Details", new PostViewModel { PostCaption = post.Text, Images = images, Likes = likes, PostComments = comments, Id = post.Id, CurrentImageIndex = currentImageIndex });


        }

    }
}
