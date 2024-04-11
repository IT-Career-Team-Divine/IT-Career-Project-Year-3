using System.ComponentModel.DataAnnotations;

namespace The_Gram.Data.Models
{
    public class Message : Content
    {

        [MaxLength(255)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Text { get; set; }
        public int SenderId { get; set; }
        public UserProfile Sender { get; set; }
        public int ReceiverId { get; set; }

        [Required]
        public UserProfile Receiver { get; set; }
        
    }
}
