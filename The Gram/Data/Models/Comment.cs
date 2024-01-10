using System.ComponentModel.DataAnnotations;

namespace The_Gram.Data.Models
{
    public class Comment : Content
    {
        [Required]
      public int PostId { get; }
        public Post Post { get; set; }
    }
}
