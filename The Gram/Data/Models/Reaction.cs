using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class Reaction
    {
        [Key]
        public int Id { get; init; }
        public User User { get; set; }
        [Required]
       public string UserId { get;}
        public Content Content { get; set; }
        [Required]
        public int ContentId { get; }
    }
}
