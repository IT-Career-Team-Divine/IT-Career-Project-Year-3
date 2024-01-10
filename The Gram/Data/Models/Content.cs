using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using The_Gram.Data.Enums;
using static The_Gram.Data.Constants.Constants.ContentConstants;
namespace The_Gram.Data.Models
{
    public abstract class Content
    {
        public Content()
        {
            this.Reactions = new HashSet<Reaction>();
            this.Comments = new HashSet<Comment>();
            this.Images = new HashSet<Image>();
        }
        [Key]
        public int Id { get; init; }
        public User User { get; set; }
        public int TotalLikes { get; set; }
        public ContentType Type { get; set; }
        [Required]
        [MaxLength(MaxContentTextLength)]
        public string Text { get; set; }
        public IEnumerable<Reaction> Reactions { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Image> Images { get; set; }
     
     [Required]
      public string UserId { get; }
    }
}
