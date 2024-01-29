using System.ComponentModel.DataAnnotations;
using static The_Gram.Data.Constants.Constants.UserConstants;
using static The_Gram.Data.Constants.Constants.ImageConstants;
using System.ComponentModel;

namespace The_Gram.Models.User
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(MaxNameLength, ErrorMessage = "{0} should be between {2} and {1} characters", MinimumLength = MinNameLength)]
        public string FullName { get; set; }
        [Required]
        [StringLength(MaxUsernameLength, ErrorMessage = "{0} should be between {2} and {1} characters", MinimumLength = MinUsernameLength)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Please enter a valid mail adress")]
        public string Email { get; set; } = null!;
        [StringLength(MaxBioLength)]
        public string? Bio { get; set;}

        [DefaultValue("https://pixabay.com/vectors/blank-profile-picture-mystery-man-973460/")]
        [MinLength(MinURLLength, ErrorMessage = "{0} should be atleast {1} characters")]
        public string? PhotoUrl { get; set; }

        [Required]
        [StringLength(MaxPasswordLength, ErrorMessage = "{0} should be between {2} and {1} characters", MinimumLength = MinPasswordLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
