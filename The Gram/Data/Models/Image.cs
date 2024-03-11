using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class Image
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string URL { get; set; }
        public Content Content { get; set; }
        [Required]
        public string ContentId { get; set; }
    }
}
