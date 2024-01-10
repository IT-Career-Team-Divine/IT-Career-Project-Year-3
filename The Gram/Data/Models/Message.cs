using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class Message : Content
    {
        public User Reciever { get; set; }
        [Required]
        public string RecieverId { get;}


    }
}
