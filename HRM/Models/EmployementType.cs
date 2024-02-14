using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class EmployementType
    {
        [Key]
        public long IntEmployementId { get; set; }
        [Required]
        public string StrEmployementName { get; set; }
    }
}
