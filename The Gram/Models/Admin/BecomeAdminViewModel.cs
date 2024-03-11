using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace The_Gram.Models.Admin
{
    public class BecomeAdminApplicationViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}