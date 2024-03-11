using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class PostCommentReaction : Reaction
    {

        [ForeignKey(nameof(Comment))]
        public string CommentId { get; set; }
        public PostComment Comment { get; set; }
    }
}
