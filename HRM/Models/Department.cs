using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class Department : IUserOwned
    {
        [Key]
        public long IntDepartmentId { get; set; }
        public long IntUserId { get; set; }

        [Required]
        public string StrDepartmentName { get; set; }
    }
}
