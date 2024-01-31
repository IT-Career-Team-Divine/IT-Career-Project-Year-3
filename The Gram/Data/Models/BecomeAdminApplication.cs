using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Gram.Data.Models
{
    public class BecomeAdminApplication
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Applicant))]
        public string ApplicantId { get; set; }
        public User Applicant { get; set; }
    }
}
