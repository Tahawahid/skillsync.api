using System.ComponentModel.DataAnnotations;

namespace skillsync.api.Models
{
    public class WorkExperience
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(50)]
        public string ExperienceLevel { get; set; } = string.Empty;

        public User User { get; set; } = null!;

        public ICollection<JobRole> JobRoles { get; set; } = new List<JobRole>();
    }
}