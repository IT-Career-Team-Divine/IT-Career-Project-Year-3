using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string URL { get; set; }
        public Content Content { get; set; }
        public int ContentId { get; set; }
    }
}
