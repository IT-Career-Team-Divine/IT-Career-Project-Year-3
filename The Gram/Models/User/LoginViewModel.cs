using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace The_Gram.Models.User
{
    public  class LoginViewModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Username { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
