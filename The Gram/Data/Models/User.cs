using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static The_Gram.Data.Constants.Constants.UserConstants;
namespace The_Gram.Data.Models
{
    public class User : IdentityUser
    {
        static string defaultPhoto = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
        public User()
        {
            this.Posts = new HashSet<Post>();
            this.Comments = new HashSet<Comment>();
            this.Friends = new HashSet<User>();
            this.Followers = new HashSet<User>();
            this.SentMessages = new HashSet<Message>();
            this.Reactions = new HashSet<Reaction>();
            this.RecievedMessages = new HashSet<Message>();
        }
        [Required]
        [MaxLength(MaxNameLength)]
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string Picture { get; set; } = defaultPhoto;
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Message> SentMessages { get; set; }
        public IEnumerable<Message> RecievedMessages { get; set; }
        public IEnumerable<User> Friends { get; set; }
        public IEnumerable<User> Followers { get; set; }
        public IEnumerable<Reaction> Reactions { get; set; }
        public BecomeAdminApplication AdminApplication { get; set; }
    }
}