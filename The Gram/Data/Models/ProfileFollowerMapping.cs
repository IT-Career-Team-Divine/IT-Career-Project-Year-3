namespace The_Gram.Data.Models
{
    public class ProfileFollowerMapping
    {
        public string Id { get; set; }
        public UserProfile Follower { get; set; }
        public UserProfile Profile { get; set; }
    }
}
