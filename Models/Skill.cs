using System.ComponentModel.DataAnnotations;

namespace skillsync.api.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Required]
        public int SkillsId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string SkillType { get; set; } = string.Empty; // Technical, Soft, ToLearn

        public Skills Skills { get; set; } = null!;
    }
}
