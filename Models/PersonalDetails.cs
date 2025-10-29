using System.ComponentModel.DataAnnotations;

namespace skillsync.api.Models
{
    public class PersonalDetails
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public int Age { get; set; }

        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [StringLength(100)]
        public string CurrentRole { get; set; } = string.Empty;

        public User User { get; set; } = null!;
    }
}