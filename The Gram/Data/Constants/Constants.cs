﻿namespace The_Gram.Data.Constants
{
    public class Constants
    {
        public string AppPassword = "roii vfmu aivu jnpw\\r\\n";
        public class UserConstants
        {
            public const string defaultBio = "Put in bio";
            public const string defaultPicture = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
            public const int MaxNameLength = 100;
            public const int MinNameLength = 2;
            public const int MaxUsernameLength = 256;
            public const int MinUsernameLength = 4;
            public const int MaxPasswordLength = 100;
            public const int MinPasswordLength = 8;
            public const int MaxBioLength = 150;
        }
        public class ImageConstants
        {
            public const int MinURLLength = 4;
        }
        public class ContentConstants
        {
            public const int MaxContentTextLength = 2200;
            public const int MinContentTextLength = 1;
        }
    }
}
