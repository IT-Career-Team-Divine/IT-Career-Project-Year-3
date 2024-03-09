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
            this.Reactions = new List<Reaction>();
            this.Images = new List<Image>();
        }
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public int TotalLikes { get; set; }
        [Required]
        [MaxLength(MaxContentTextLength)]
        public string Text { get; set; }
        public List<Reaction> Reactions { get; set; }
        public List<Image> Images { get; set; }
     
    }
}
