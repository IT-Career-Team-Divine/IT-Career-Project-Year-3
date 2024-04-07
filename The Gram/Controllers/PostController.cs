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
        private readonly IAdminService adminService;
        public PostController(IPostService _postService, IUserService _userService, UserManager<User> _userManager, IAdminService _adminService)
        {
            postService = _postService;
            userService = _userService;
            userManager = _userManager;
            adminService = _adminService;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Post()
        {
            var model = new PostCreationViewModel();
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(PostCreationViewModel model, string[] imageUrls)
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var currentProfile = await userService.GetProfileByIdAsync(currentUser.CurrentProfileId);
            var result = await postService.CreatePost(model, imageUrls, currentProfile);
            if (!result)
            {
                return View(model);
            }
            return RedirectToAction("Index", "Home");

        }
        [Authorize]
        public async Task<IActionResult> Feed()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return View("Index", "Home");
            }
            var user = await userManager.GetUserAsync(HttpContext.User);

            var profile = await userService.GetProfileByIdAsync(user.CurrentProfileId);
            List<AllPostsViewModel> posts = await postService.GetFeedAsync(profile.Id);
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
        public async Task<IActionResult> PrevImage(string thisPostId, int currentImageIndex)
        {
            int prevIndex = 0;
            var post = await postService.GetByIdAsync(thisPostId);

            if (post == null)
            {
                return NotFound();
            }

            var images = await postService.GetPostImages(thisPostId);
            var likes = await postService.GetPostLikes(thisPostId);
            var dislikes = await postService.GetPostDislikes(thisPostId);
            var comments = await postService.getPostComments(thisPostId);

            if (currentImageIndex > 0)
            {
                prevIndex = currentImageIndex - 1;

            }
            else
            {
                prevIndex = 0;
            }

            var imageUrl = images[prevIndex].URL;

            return View("Details", new PostViewModel { PostCaption = post.Text, Images = images, Likes = likes, Dislikes = dislikes, PostComments = comments, Id = post.Id, CurrentImageIndex = currentImageIndex });
        }

        [HttpGet]
        public async Task<IActionResult> NextImage(string thisPostId, int currentImageIndex)
        {
            int nextIndex = 0;
            var post = await postService.GetByIdAsync(thisPostId);

            if (post == null)
            {
                return NotFound();
            }

            var images = await postService.GetPostImages(thisPostId);
            var likes = await postService.GetPostLikes(thisPostId);
            var dislikes = await postService.GetPostDislikes(thisPostId);
            var comments = await postService.getPostComments(thisPostId);

            if (currentImageIndex != images.Count - 1)
            {
                nextIndex = currentImageIndex + 1;
            }

            var imageUrl = images[nextIndex].URL;


            return View("Details", new PostViewModel { PostCaption = post.Text, Images = images, Likes = likes, Dislikes = dislikes, PostComments = comments, Id = post.Id, CurrentImageIndex = currentImageIndex });
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Like(string thisPostId, int currentImageIndex)
        {
            var post = await postService.GetByIdAsync(thisPostId);
            var user = await userService.GetProfileByIdAsync(post.UserId);
            var images = await postService.GetPostImages(thisPostId);
            var comments = await postService.getPostComments(thisPostId);

            await postService.Like(post, user);

            var dislikes = await postService.GetPostDislikes(thisPostId);
            var likes = await postService.GetPostLikes(thisPostId);

            return View("Details", new PostViewModel { PostCaption = post.Text, Images = images, Likes = likes, Dislikes = dislikes, PostComments = post.Comments, Id = post.Id, CurrentImageIndex = currentImageIndex });


        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Dislike(string thisPostId, int currentImageIndex)
        {
            var post = await postService.GetByIdAsync(thisPostId);
            var user = await userService.GetProfileByIdAsync(post.UserId);
            var images = await postService.GetPostImages(thisPostId);
            var comments = await postService.getPostComments(thisPostId);

            await postService.Dislike(post, user);

            var likes = await postService.GetPostLikes(thisPostId);
            var dislikes = await postService.GetPostDislikes(thisPostId);

            return View("Details", new PostViewModel { PostCaption = post.Text, Images = images, Likes = likes, Dislikes = dislikes, PostComments = post.Comments, Id = post.Id, CurrentImageIndex = currentImageIndex });


        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(string thisPostId, int currentImageIndex, string currentProfileId, string commentText)
        {
            var post = await postService.GetByIdAsync(thisPostId);
            var user = await userService.GetProfileByIdAsync(currentProfileId);
            var images = await postService.GetPostImages(thisPostId);
            var likes = await postService.GetPostLikes(thisPostId);
            var dislikes = await postService.GetPostDislikes(thisPostId);

            await postService.Comment(post, user, commentText);
            List<PostComment> comments = await postService.getPostComments(thisPostId);

            return View("Details", new PostViewModel { PostCaption = post.Text, Images = images, Likes = likes, Dislikes = dislikes, PostComments = post.Comments, Id = post.Id, CurrentImageIndex = currentImageIndex });


        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id, string currentProfileId)
        {
            var post = await postService.GetByIdAsync(id);
            var profile = await userService.GetProfileByIdAsync(currentProfileId);

            var user = await userService.GetByIdAsync(profile.UserId);
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var userIsAdmin = await adminService.IsAdminAsync(user, profile);

            if (currentUser.Id != post.UserId && !userIsAdmin)
            {
                return RedirectToAction("Become", "Admin");
            }
            var postCreationView = new PostCreationViewModel()
            {
                Id = id,
                Description = post.Text,
            };
            return View(postCreationView);
        }
    }
}