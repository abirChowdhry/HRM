using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class AppUser
    {
        [Key]
        public long IntUserId { get; set; }
        [Required]
        public string StrFullName { get; set; }
        [Required]
        public string StrEmail { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public DateTime DteCreatedAt { get; set; }
    }
}
