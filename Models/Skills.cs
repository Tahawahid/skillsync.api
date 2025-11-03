using System.ComponentModel.DataAnnotations;

namespace skillsync.api.Models
{
    public class Skills
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public ICollection<Skill> TechnicalSkills { get; set; } = new List<Skill>();
        public ICollection<Skill> SoftSkills { get; set; } = new List<Skill>();
        public ICollection<Skill> SkillsToLearn { get; set; } = new List<Skill>();
    }
}
