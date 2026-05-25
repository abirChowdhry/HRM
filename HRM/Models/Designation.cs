using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class Designation : IUserOwned
    {
        [Key]
        public long IntDesignationId { get; set; }
        public long IntUserId { get; set; }
        [Required]
        public string StrDesignationName { get; set; }
    }
}
