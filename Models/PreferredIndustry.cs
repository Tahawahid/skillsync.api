using System.ComponentModel.DataAnnotations;

namespace skillsync.api.Models
{
    public class PreferredIndustry
    {
        public int Id { get; set; }

        [Required]
        public int CareerGoalsId { get; set; }

        [Required]
        [StringLength(100)]
        public string Industry { get; set; } = string.Empty;

        public CareerGoals CareerGoals { get; set; } = null!;
    }
}