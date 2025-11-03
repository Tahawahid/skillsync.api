namespace skillsync.api.Dtos
{
    public class PersonalDetailsDto
    {
        public int Age { get; set; }
        public string Location { get; set; } = string.Empty;
        public string CurrentRole { get; set; } = string.Empty;
    }

    public class EducationDto
    {
        public string HighestEducation { get; set; } = string.Empty;
        public string FieldOfStudy { get; set; } = string.Empty;
        public int? GraduationYear { get; set; }
        public List<string> Certifications { get; set; } = new List<string>();
    }

    public class WorkExperienceDto
    {
        public string ExperienceLevel { get; set; } = string.Empty;
        public List<JobRoleDto> JobRoles { get; set; } = new List<JobRoleDto>();
    }

    public class JobRoleDto
    {
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string JobDescription { get; set; } = string.Empty;
    }

    public class SkillsDto
    {
        public List<string> TechnicalSkills { get; set; } = new List<string>();
        public List<string> SoftSkills { get; set; } = new List<string>();
        public List<string> SkillsToLearn { get; set; } = new List<string>();
    }

    public class CareerGoalsDto
    {
        public List<string> Goals { get; set; } = new List<string>();
        public string Timeframe { get; set; } = string.Empty;
        public List<string> PreferredIndustries { get; set; } = new List<string>();
        public string WorkPreference { get; set; } = string.Empty;
    }

    public class OnboardingCompleteDto
    {
        public int UserId { get; set; }
        public PersonalDetailsDto PersonalDetails { get; set; } = null!;
        public EducationDto Education { get; set; } = null!;
        public WorkExperienceDto WorkExperience { get; set; } = null!;
        public SkillsDto Skills { get; set; } = null!;
        public CareerGoalsDto CareerGoals { get; set; } = null!;
    }

    public class OnboardingResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}