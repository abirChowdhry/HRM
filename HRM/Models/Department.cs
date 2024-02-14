using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class Department
    {
        [Key]
        public long IntDepartmentId { get; set; }

        [Required]
        public string StrDepartmentName { get; set; }
    }
}
