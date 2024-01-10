namespace The_Gram.Data.Models
{
    public class Post : Content
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
        }
        public IEnumerable<Comment> Comments { get; set; }

    }
}
