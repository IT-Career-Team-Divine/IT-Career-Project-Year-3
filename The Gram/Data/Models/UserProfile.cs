using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static The_Gram.Data.Constants.Constants.UserConstants;
using System.ComponentModel.DataAnnotations.Schema;
using The_Gram.Data.Models.The_Gram.Data.Models;

namespace The_Gram.Data.Models
{
    public class UserProfile
    {
        static string defaultPhoto = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
        public UserProfile()
        {
            this.Posts = new List<Post>();
            this.Comments = new List<PostComment>();
            this.Friends = new List<ProfileFriendMapping>();
            this.Followers = new List<ProfileFollowerMapping>();
            this.Reactions = new List<PostReaction>();
            this.PostCommentReactions = new List<PostCommentReaction>();
        }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(MaxUsernameLength)]
        public string Username { get; set; }
        [Required]
        [MaxLength(MaxNameLength)]
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string Picture { get; set; } = defaultPhoto;
        public List<Post> Posts { get; set; }
        public List<PostComment> Comments { get; set; }
        public List<ProfileFriendMapping> Friends { get; set; }
        public List<ProfileFollowerMapping> Followers { get; set; }
        public List<ProfileFollowerMapping> Following { get; set; }
        public List<PostReaction> Reactions { get; set; }
        public List<PostCommentReaction> PostCommentReactions { get; set; }
        public BecomeAdminApplication AdminApplication { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPrivate { get; set; }
    }
}
