using The_Gram.Data.Models;

namespace The_Gram.Models.Post
{
    public class PostViewModel
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string PostId { get; set; }
        public string PostCaption { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public string AuthorId { get; set; }
        public List<PostComment> PostComments { get; set; }
        public List<PostReaction> Likes { get; set; }
        public int CurrentImageIndex { get; set; } = 0;
    }
}