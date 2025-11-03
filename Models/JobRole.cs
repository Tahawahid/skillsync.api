using System.ComponentModel.DataAnnotations;

namespace skillsync.api.Models
{
    public class JobRole
    {
        public int Id { get; set; }

        [Required]
        public int WorkExperienceId { get; set; }

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [StringLength(500)]
        public string JobDescription { get; set; } = string.Empty;

        public WorkExperience WorkExperience { get; set; } = null!;
    }
}