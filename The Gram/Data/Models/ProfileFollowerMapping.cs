using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class ProfileFollowerMapping
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public UserProfile Follower { get; set; }
        [ForeignKey(nameof(Follower))]
        public string FollowerId { get; set; }
        public UserProfile Profile { get; set; }
        public string ProfileId { get; set; }

    }
}
