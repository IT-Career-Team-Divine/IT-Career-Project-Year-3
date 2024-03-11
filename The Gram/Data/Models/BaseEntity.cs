using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Channels;

namespace The_Gram.Data.Models
{
    public class BaseEntity
    {
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public DateTime DeletedOn { get; set; }
        public UserProfile? CreatedBy { get; set; }

        public UserProfile? UpdatedBy { get; set; }

        public UserProfile? DeletedBy { get; set; }
    }
}
