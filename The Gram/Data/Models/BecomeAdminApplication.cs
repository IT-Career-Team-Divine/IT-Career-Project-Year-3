using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class BecomeAdminApplication
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey(nameof(Applicant))]
        public string ApplicantId { get; set; }
        public UserProfile Applicant { get; set; }
    }
}
