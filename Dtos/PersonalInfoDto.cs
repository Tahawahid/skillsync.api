namespace skillsync.api.Dtos
{
    public class PersonalInfoDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public PersonalDetailsDto? PersonalDetails { get; set; }
        public EducationDto? Education { get; set; }
        public WorkExperienceDto? WorkExperience { get; set; }
        public SkillsDto? Skills { get; set; }
        public CareerGoalsDto? CareerGoals { get; set; }
    }

    public class UpdatePersonalInfoDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public UpdatePersonalDetailsDto? PersonalDetails { get; set; }
        public UpdateEducationDto? Education { get; set; }
        public UpdateSkillsDto? Skills { get; set; }
        public UpdateCareerGoalsDto? CareerGoals { get; set; }
        public UpdateWorkExperienceDto? WorkExperience { get; set; }
    }

    public class UpdatePersonalDetailsDto
    {
        public int? Age { get; set; }
        public string? Location { get; set; }
        public string? CurrentRole { get; set; }
    }

    public class UpdateEducationDto
    {
        public string? HighestEducation { get; set; }
        public string? FieldOfStudy { get; set; }
        public int? GraduationYear { get; set; }
        public List<string>? Certifications { get; set; }
    }

    public class UpdateWorkExperienceDto
    {
        public string? ExperienceLevel { get; set; }
        public List<JobRoleDto>? JobRoles { get; set; }
    }

    public class UpdateSkillsDto
    {
        public List<string>? TechnicalSkills { get; set; }
        public List<string>? SoftSkills { get; set; }
        public List<string>? SkillsToLearn { get; set; }
    }

    public class UpdateCareerGoalsDto
    {
        public List<string>? Goals { get; set; }
        public string? Timeframe { get; set; }
        public List<string>? PreferredIndustries { get; set; }
        public string? WorkPreference { get; set; }
    }

    public class UpdatePersonalInfoResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}