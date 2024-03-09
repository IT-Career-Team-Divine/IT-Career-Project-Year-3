using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class PostReaction : Reaction
    {
        public Post Post { get; set; }
        [ForeignKey(nameof(Post))]
        public string PostId { get; set; }
    }
}
