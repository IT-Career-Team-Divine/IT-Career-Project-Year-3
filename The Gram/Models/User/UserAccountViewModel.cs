using The_Gram.Models.Post;

namespace The_Gram.Models.User
{
    public class UserAccountViewModel
    {
        public UserAccountViewModel()
        {
                this.UserPosts = new List<AllPostsViewModel>();
        }
        public string Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string PictureUr { get; set; }
        public bool IsPrivate { get; set; }
        public List<AllPostsViewModel> UserPosts { get; set; }
    }
}
