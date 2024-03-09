using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class Reaction
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public UserProfile User { get; set; }
        [ForeignKey(nameof(User))]
       public string UserId { get; set; }
    }
}
