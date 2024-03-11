using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class PostComment : Comment
    {
        public PostComment()
        {
            Reactions = new List<PostCommentReaction>();
            Replies = new List<PostComment>();
        }
        [ForeignKey(nameof(Post))]
        public string PostId { get; set; }
        public Post Post { get; set; }

        public List<PostCommentReaction> Reactions { get; set; }

        public List<PostComment> Replies { get; set; }
    }
}
