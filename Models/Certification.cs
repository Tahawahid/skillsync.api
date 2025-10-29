using System.ComponentModel.DataAnnotations;

namespace skillsync.api.Models
{
    public class Certification
    {
        public int Id { get; set; }

        [Required]
        public int EducationId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public Education Education { get; set; } = null!;
    }
}