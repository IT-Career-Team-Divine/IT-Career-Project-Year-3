using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static The_Gram.Data.Constants.Constants.UserConstants;
namespace The_Gram.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
        }
        [ForeignKey(nameof(CurrentProfile))]
        public string CurrentProfileId { get; set; }
        public UserProfile CurrentProfile { get; set; }
    }
}
