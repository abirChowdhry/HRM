using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class EmployementType : IUserOwned
    {
        [Key]
        public long IntEmployementId { get; set; }
        public long IntUserId { get; set; }
        [Required]
        public string StrEmployementName { get; set; }
    }
}
