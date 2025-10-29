using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace skillsync.api.Models
{
    [Table("User_UserLogin")]
    public class UserLogin
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string UserName { get; set; }

        [Required, MaxLength(250)]
        public string Email { get; set; }

        // Store hashed password
        [Required, MaxLength(200)]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
