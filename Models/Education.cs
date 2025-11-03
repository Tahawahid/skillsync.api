using System.ComponentModel.DataAnnotations;

namespace skillsync.api.Models
{
    public class Education
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(100)]
        public string HighestEducation { get; set; } = string.Empty;

        [StringLength(100)]
        public string FieldOfStudy { get; set; } = string.Empty;

        public int? GraduationYear { get; set; }

        public User User { get; set; } = null!;

        public ICollection<Certification> Certifications { get; set; } = new List<Certification>();
    }
}