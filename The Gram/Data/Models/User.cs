using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static The_Gram.Data.Constants.Constants.UserConstants;
namespace The_Gram.Data.Models
{
    public class User : IdentityUser
    {
         string defaultPhoto = "https://pixabay.com/vectors/blank-profile-picture-mystery-man-973460/";
         string defaultBio = "Put in bio";
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
        [Required]
        [MaxLength(MaxNameLength)]
        public string FullName{ get; set; }
        public string Bio { get { return defaultBio; } set { defaultBio = value; } }
        public string Picture { get { return defaultPhoto; } set { defaultPhoto = value; } }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Message> SentMessages { get; set; }
        public IEnumerable<Message> RecievedMessages { get; set; }
        public IEnumerable<User> Friends { get; set; }
        public IEnumerable<User> Followers { get; set; }
        public IEnumerable<Reaction> Reactions { get; set; }
    }
}
