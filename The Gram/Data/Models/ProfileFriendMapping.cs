﻿namespace The_Gram.Data.Models
{
    public class ProfileFriendMapping
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public long Timestamp { get; set; }
        public UserProfile Profile { get; set; }
        public UserProfile Friend { get; set; }
    }
}