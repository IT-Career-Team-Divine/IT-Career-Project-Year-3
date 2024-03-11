using The_Gram.Data.Models;

namespace The_Gram.Models.Post
{
    public class AllPostsViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public string Author { get; set; }
    }
}
