using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class BusinessUnit : IUserOwned
    {
        [Key]
        public long IntBusinessUnitId { get; set; }
        public long IntUserId { get; set; }
        [Required]
        public string StrBusinessUnitName { get; set; }
    }
}
