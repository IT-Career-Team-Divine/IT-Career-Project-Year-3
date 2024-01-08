using Microsoft.AspNetCore.Identity;

namespace The_Gram.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Contents = new HashSet<Content>();
            this.Friends = new HashSet<User>();
            this.Followers = new HashSet<User>();
            this.Messages= new HashSet<Message>();
            this.Reactions= new HashSet<Reaction>();
        }
        public string Bio { get; set; }
        public IEnumerable<Content> Contents { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<User> Friends { get; set; }
        public IEnumerable<User> Followers { get; set; }
        public IEnumerable<Reaction> Reactions { get; set; }
    }
}
