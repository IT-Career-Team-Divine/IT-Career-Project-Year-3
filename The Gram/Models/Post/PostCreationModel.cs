using System.ComponentModel.DataAnnotations;
using The_Gram.Data.Models;

namespace The_Gram.Models.Post
{
    public class PostCreationViewModel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Description { get; set; }
    }
}