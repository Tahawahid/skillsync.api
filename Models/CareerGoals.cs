using System.ComponentModel.DataAnnotations;

namespace skillsync.api.Models
{
    public class CareerGoals
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(50)]
        public string Timeframe { get; set; } = string.Empty;

        [StringLength(50)]
        public string WorkPreference { get; set; } = string.Empty;

        public User User { get; set; } = null!;

        public ICollection<CareerGoal> Goals { get; set; } = new List<CareerGoal>();
        public ICollection<PreferredIndustry> PreferredIndustries { get; set; } = new List<PreferredIndustry>();
    }
}