using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class Comment
    {
        [Key]
       public string Id { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey(nameof(Commenter))]
        public string CommenterId { get; set; }
        public UserProfile Commenter { get; set; }
        public string Content { get; set; }
    }
}
