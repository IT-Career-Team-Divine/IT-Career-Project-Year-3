using Microsoft.AspNetCore.Identity;

namespace The_Gram.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Posts = new HashSet<Post>();
            this.Comments = new HashSet<Comment>();
            this.Friends = new HashSet<User>();
            this.Followers = new HashSet<User>();
            this.SentMessages= new HashSet<Message>();
            this.Reactions= new HashSet<Reaction>();
            this.RecievedMessages = new HashSet<Message>();
        }
        public string Bio { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Message> SentMessages { get; set; }
        public IEnumerable<Message> RecievedMessages { get; set; }
        public IEnumerable<User> Friends { get; set; }
        public IEnumerable<User> Followers { get; set; }
        public IEnumerable<Reaction> Reactions { get; set; }
    }
}
