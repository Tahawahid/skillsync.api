using System.ComponentModel.DataAnnotations;

namespace skillsync.api.Models
{
    public class CareerGoal
    {
        public int Id { get; set; }

        [Required]
        public int CareerGoalsId { get; set; }

        [Required]
        [StringLength(100)]
        public string Goal { get; set; } = string.Empty;

        public CareerGoals CareerGoals { get; set; } = null!;
    }
}