using Microsoft.EntityFrameworkCore;
using skillsync.api.Data;
using skillsync.api.Dtos;
using skillsync.api.Models;

namespace skillsync.api.Services
{
    public class OnboardingService : IOnboardingService
    {
        private readonly AppDbContext _context;

        public OnboardingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OnboardingResponseDto> CompleteOnboardingAsync(int userId, OnboardingCompleteDto onboardingData)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Save Personal Details
                var personalDetails = new PersonalDetails
                {
                    UserId = userId,
                    Age = onboardingData.PersonalDetails.Age,
                    Location = onboardingData.PersonalDetails.Location,
                    CurrentRole = onboardingData.PersonalDetails.CurrentRole
                };
                _context.PersonalDetails.Add(personalDetails);

                // Save Education
                var education = new Education
                {
                    UserId = userId,
                    HighestEducation = onboardingData.Education.HighestEducation,
                    FieldOfStudy = onboardingData.Education.FieldOfStudy,
                    GraduationYear = onboardingData.Education.GraduationYear
                };
                _context.Education.Add(education);
                await _context.SaveChangesAsync();

                // Save Certifications
                foreach (var cert in onboardingData.Education.Certifications)
                {
                    var certification = new Certification
                    {
                        EducationId = education.Id,
                        Name = cert
                    };
                    _context.Certifications.Add(certification);
                }

                // Save Work Experience
                var workExperience = new WorkExperience
                {
                    UserId = userId,
                    ExperienceLevel = onboardingData.WorkExperience.ExperienceLevel
                };
                _context.WorkExperience.Add(workExperience);
                await _context.SaveChangesAsync();

                // Save Job Roles
                foreach (var role in onboardingData.WorkExperience.JobRoles)
                {
                    var jobRole = new JobRole
                    {
                        WorkExperienceId = workExperience.Id,
                        JobTitle = role.JobTitle,
                        CompanyName = role.CompanyName,
                        StartDate = role.StartDate,
                        EndDate = role.EndDate,
                        JobDescription = role.JobDescription
                    };
                    _context.JobRoles.Add(jobRole);
                }

                // Save Skills
                var skills = new Skills
                {
                    UserId = userId
                };
                _context.Skills.Add(skills);
                await _context.SaveChangesAsync();

                // Save Technical Skills
                foreach (var skill in onboardingData.Skills.TechnicalSkills)
                {
                    var skillEntity = new Skill
                    {
                        SkillsId = skills.Id,
                        Name = skill,
                        SkillType = "Technical"
                    };
                    _context.Skill.Add(skillEntity);
                }

                // Save Soft Skills
                foreach (var skill in onboardingData.Skills.SoftSkills)
                {
                    var skillEntity = new Skill
                    {
                        SkillsId = skills.Id,
                        Name = skill,
                        SkillType = "Soft"
                    };
                    _context.Skill.Add(skillEntity);
                }

                // Save Skills to Learn
                foreach (var skill in onboardingData.Skills.SkillsToLearn)
                {
                    var skillEntity = new Skill
                    {
                        SkillsId = skills.Id,
                        Name = skill,
                        SkillType = "ToLearn"
                    };
                    _context.Skill.Add(skillEntity);
                }

                // Save Career Goals
                var careerGoals = new CareerGoals
                {
                    UserId = userId,
                    Timeframe = onboardingData.CareerGoals.Timeframe,
                    WorkPreference = onboardingData.CareerGoals.WorkPreference
                };
                _context.CareerGoals.Add(careerGoals);
                await _context.SaveChangesAsync();

                // Save Goals
                foreach (var goal in onboardingData.CareerGoals.Goals)
                {
                    var careerGoal = new CareerGoal
                    {
                        CareerGoalsId = careerGoals.Id,
                        Goal = goal
                    };
                    _context.CareerGoal.Add(careerGoal);
                }

                // Save Preferred Industries
                foreach (var industry in onboardingData.CareerGoals.PreferredIndustries)
                {
                    var preferredIndustry = new PreferredIndustry
                    {
                        CareerGoalsId = careerGoals.Id,
                        Industry = industry
                    };
                    _context.PreferredIndustry.Add(preferredIndustry);
                }

                // Update user onboarding status
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.IsOnboardingCompleted = true;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new OnboardingResponseDto
                {
                    Success = true,
                    Message = "Onboarding completed successfully"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new OnboardingResponseDto
                {
                    Success = false,
                    Message = "Failed to complete onboarding: " + ex.Message
                };
            }
        }

        public async Task<OnboardingCompleteDto?> GetOnboardingDataAsync(int userId)
        {
            var personalDetails = await _context.PersonalDetails
                .FirstOrDefaultAsync(pd => pd.UserId == userId);

            var education = await _context.Education
                .Include(e => e.Certifications)
                .FirstOrDefaultAsync(e => e.UserId == userId);

            var workExperience = await _context.WorkExperience
                .Include(we => we.JobRoles)
                .FirstOrDefaultAsync(we => we.UserId == userId);

            var skills = await _context.Skills
                .Include(s => s.TechnicalSkills)
                .Include(s => s.SoftSkills)
                .Include(s => s.SkillsToLearn)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            var careerGoals = await _context.CareerGoals
                .Include(cg => cg.Goals)
                .Include(cg => cg.PreferredIndustries)
                .FirstOrDefaultAsync(cg => cg.UserId == userId);

            if (personalDetails == null || education == null || workExperience == null ||
                skills == null || careerGoals == null)
            {
                return null;
            }

            return new OnboardingCompleteDto
            {
                PersonalDetails = new PersonalDetailsDto
                {
                    Age = personalDetails.Age,
                    Location = personalDetails.Location,
                    CurrentRole = personalDetails.CurrentRole
                },
                Education = new EducationDto
                {
                    HighestEducation = education.HighestEducation,
                    FieldOfStudy = education.FieldOfStudy,
                    GraduationYear = education.GraduationYear,
                    Certifications = education.Certifications.Select(c => c.Name).ToList()
                },
                WorkExperience = new WorkExperienceDto
                {
                    ExperienceLevel = workExperience.ExperienceLevel,
                    JobRoles = workExperience.JobRoles.Select(jr => new JobRoleDto
                    {
                        JobTitle = jr.JobTitle,
                        CompanyName = jr.CompanyName,
                        StartDate = jr.StartDate,
                        EndDate = jr.EndDate,
                        JobDescription = jr.JobDescription
                    }).ToList()
                },
                Skills = new SkillsDto
                {
                    TechnicalSkills = skills.TechnicalSkills.Select(s => s.Name).ToList(),
                    SoftSkills = skills.SoftSkills.Select(s => s.Name).ToList(),
                    SkillsToLearn = skills.SkillsToLearn.Select(s => s.Name).ToList()
                },
                CareerGoals = new CareerGoalsDto
                {
                    Goals = careerGoals.Goals.Select(g => g.Goal).ToList(),
                    Timeframe = careerGoals.Timeframe,
                    PreferredIndustries = careerGoals.PreferredIndustries.Select(pi => pi.Industry).ToList(),
                    WorkPreference = careerGoals.WorkPreference
                }
            };
        }
    }
}