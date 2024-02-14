using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class BusinessUnit
    {
        [Key]
        public long IntBusinessUnitId { get; set; }
        [Required]
        public string StrBusinessUnitName { get; set; }
    }
}
