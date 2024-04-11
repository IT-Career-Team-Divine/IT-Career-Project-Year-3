using System.ComponentModel.DataAnnotations;

namespace The_Gram.Models.Chat
{
    public class ChatMessageViewModel
    {
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }

    }
}
