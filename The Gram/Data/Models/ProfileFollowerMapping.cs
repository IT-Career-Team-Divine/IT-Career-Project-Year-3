using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class ProfileFollowerMapping
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public UserProfile Follower { get; set; }
        public string FollowerId { get; set; }
        public UserProfile Following { get; set; }
        public string FollowingId { get; set; }

    }
}
