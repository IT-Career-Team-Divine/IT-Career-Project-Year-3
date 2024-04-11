using System.ComponentModel.DataAnnotations;

namespace The_Gram.Models.Chat
{
    public class SendMessageViewModel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Message { get; set; }
    }
}
