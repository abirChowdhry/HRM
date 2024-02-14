using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class Designation
    {
        [Key]
        public long IntDesignationId { get; set; }
        [Required]
        public string StrDesignationName { get; set; }
    }
}
