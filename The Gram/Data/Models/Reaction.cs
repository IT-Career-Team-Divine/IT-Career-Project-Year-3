using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class Reaction
    {
        [Key]
        public int Id { get; init; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [Required]
        public string UserId { get;}
        public Content Content { get; set; }
        public int ContentId { get; }
    }
}
