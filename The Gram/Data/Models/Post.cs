namespace The_Gram.Data.Models
{
    public class Post : Content
    {
        public Post()
        {
            this.Comments = new List<PostComment>();
            this.Reactions= new List<PostReaction>();
        }
        public List<PostComment> Comments { get; set; }
        public List<PostReaction> Reactions { get; set; }

    }
}
