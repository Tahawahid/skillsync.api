using Microsoft.EntityFrameworkCore;
using skillsync.api.Data;
using skillsync.api.Dtos;
using skillsync.api.Models;

namespace skillsync.api.Services
{
    public class PersonalInfoService : IPersonalInfoService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PersonalInfoService> _logger;

        public PersonalInfoService(AppDbContext context, ILogger<PersonalInfoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PersonalInfoDto?> GetUserInfoAsync(int userId)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return null;
                }

                // Get personal details
                var personalDetails = await _context.PersonalDetails
                    .FirstOrDefaultAsync(pd => pd.UserId == userId);

                // Get education
                var education = await _context.Education
                    .FirstOrDefaultAsync(e => e.UserId == userId);

                // Get certifications
                var certifications = new List<string>();
                if (education != null)
                {
                    certifications = await _context.Certifications
                        .Where(c => c.EducationId == education.Id)
                        .Select(c => c.Name)
                        .ToListAsync();
                }

                // Get work experience
                var workExperience = await _context.WorkExperience
                    .FirstOrDefaultAsync(we => we.UserId == userId);

                // Get job roles
                var jobRoles = new List<JobRoleDto>();
                if (workExperience != null)
                {
                    jobRoles = await _context.JobRoles
                        .Where(jr => jr.WorkExperienceId == workExperience.Id)
                        .Select(jr => new JobRoleDto
                        {
                            JobTitle = jr.JobTitle,
                            CompanyName = jr.CompanyName,
                            StartDate = jr.StartDate,
                            EndDate = jr.EndDate,
                            JobDescription = jr.JobDescription
                        })
                        .ToListAsync();
                }

                // Get Skills record for this user
                var skills = await _context.Skills
                    .FirstOrDefaultAsync(s => s.UserId == userId);

                var technicalSkills = new List<string>();
                var softSkills = new List<string>();
                var skillsToLearn = new List<string>();

                if (skills != null)
                {
                    // Get skills by type using the SkillsId
                    technicalSkills = await _context.Skill
                        .Where(s => s.SkillsId == skills.Id && s.SkillType == "Technical")
                        .Select(s => s.Name)
                        .ToListAsync();

                    softSkills = await _context.Skill
                        .Where(s => s.SkillsId == skills.Id && s.SkillType == "Soft")
                        .Select(s => s.Name)
                        .ToListAsync();

                    skillsToLearn = await _context.Skill
                        .Where(s => s.SkillsId == skills.Id && s.SkillType == "ToLearn")
                        .Select(s => s.Name)
                        .ToListAsync();
                }

                // Get career goals
                var careerGoals = await _context.CareerGoals
                    .FirstOrDefaultAsync(cg => cg.UserId == userId);

                var goals = new List<string>();
                var industries = new List<string>();

                if (careerGoals != null)
                {
                    // Get goals list using CareerGoalsId
                    goals = await _context.CareerGoal
                        .Where(cg => cg.CareerGoalsId == careerGoals.Id)
                        .Select(cg => cg.Goal)
                        .ToListAsync();

                    // Get preferred industries using CareerGoalsId
                    industries = await _context.PreferredIndustry
                        .Where(pi => pi.CareerGoalsId == careerGoals.Id)
                        .Select(pi => pi.Industry)
                        .ToListAsync();
                }

                return new PersonalInfoDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PersonalDetails = personalDetails != null ? new PersonalDetailsDto
                    {
                        Age = personalDetails.Age,
                        Location = personalDetails.Location,
                        CurrentRole = personalDetails.CurrentRole
                    } : null,
                    Education = education != null ? new EducationDto
                    {
                        HighestEducation = education.HighestEducation,
                        FieldOfStudy = education.FieldOfStudy,
                        GraduationYear = education.GraduationYear,
                        Certifications = certifications
                    } : null,
                    WorkExperience = workExperience != null ? new WorkExperienceDto
                    {
                        ExperienceLevel = workExperience.ExperienceLevel,
                        JobRoles = jobRoles
                    } : null,
                    Skills = new SkillsDto
                    {
                        TechnicalSkills = technicalSkills,
                        SoftSkills = softSkills,
                        SkillsToLearn = skillsToLearn
                    },
                    CareerGoals = careerGoals != null ? new CareerGoalsDto
                    {
                        Goals = goals,
                        Timeframe = careerGoals.Timeframe,
                        PreferredIndustries = industries,
                        WorkPreference = careerGoals.WorkPreference
                    } : null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user info for user {UserId}", userId);
                throw;
            }
        }

        public async Task<UpdatePersonalInfoResponseDto> UpdateUserInfoAsync(int userId, UpdatePersonalInfoDto updateData)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return new UpdatePersonalInfoResponseDto
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Update basic user info
                if (!string.IsNullOrEmpty(updateData.FirstName))
                    user.FirstName = updateData.FirstName;

                if (!string.IsNullOrEmpty(updateData.LastName))
                    user.LastName = updateData.LastName;

                if (!string.IsNullOrEmpty(updateData.Email))
                    user.Email = updateData.Email;

                // Update personal details
                if (updateData.PersonalDetails != null)
                {
                    var personalDetails = await _context.PersonalDetails
                        .FirstOrDefaultAsync(pd => pd.UserId == userId);

                    if (personalDetails == null)
                    {
                        personalDetails = new PersonalDetails
                        {
                            UserId = userId
                        };
                        _context.PersonalDetails.Add(personalDetails);
                    }

                    if (updateData.PersonalDetails.Age.HasValue)
                        personalDetails.Age = updateData.PersonalDetails.Age.Value;

                    if (!string.IsNullOrEmpty(updateData.PersonalDetails.Location))
                        personalDetails.Location = updateData.PersonalDetails.Location;

                    if (!string.IsNullOrEmpty(updateData.PersonalDetails.CurrentRole))
                        personalDetails.CurrentRole = updateData.PersonalDetails.CurrentRole;
                }

                // Update education
                if (updateData.Education != null)
                {
                    var education = await _context.Education
                        .FirstOrDefaultAsync(e => e.UserId == userId);

                    if (education == null)
                    {
                        education = new Education
                        {
                            UserId = userId
                        };
                        _context.Education.Add(education);
                        await _context.SaveChangesAsync(); // Save to get the ID
                    }

                    if (!string.IsNullOrEmpty(updateData.Education.HighestEducation))
                        education.HighestEducation = updateData.Education.HighestEducation;

                    if (!string.IsNullOrEmpty(updateData.Education.FieldOfStudy))
                        education.FieldOfStudy = updateData.Education.FieldOfStudy;

                    if (updateData.Education.GraduationYear.HasValue)
                        education.GraduationYear = updateData.Education.GraduationYear.Value;

                    // Update certifications
                    if (updateData.Education.Certifications != null)
                    {
                        // Remove existing certifications
                        var existingCertifications = await _context.Certifications
                            .Where(c => c.EducationId == education.Id)
                            .ToListAsync();

                        if (existingCertifications.Any())
                        {
                            _context.Certifications.RemoveRange(existingCertifications);
                        }

                        // Add new certifications
                        var newCertifications = updateData.Education.Certifications
                            .Where(c => !string.IsNullOrEmpty(c))
                            .Select(c => new Certification
                            {
                                Name = c,
                                EducationId = education.Id
                            }).ToList();

                        if (newCertifications.Any())
                        {
                            _context.Certifications.AddRange(newCertifications);
                        }
                    }
                }

                // Update work experience
                if (updateData.WorkExperience != null)
                {
                    var workExperience = await _context.WorkExperience
                        .FirstOrDefaultAsync(we => we.UserId == userId);

                    if (workExperience == null)
                    {
                        workExperience = new WorkExperience
                        {
                            UserId = userId
                        };
                        _context.WorkExperience.Add(workExperience);
                        await _context.SaveChangesAsync(); // Save to get the ID
                    }

                    if (!string.IsNullOrEmpty(updateData.WorkExperience.ExperienceLevel))
                        workExperience.ExperienceLevel = updateData.WorkExperience.ExperienceLevel;

                    // Update job roles
                    if (updateData.WorkExperience.JobRoles != null)
                    {
                        // Remove existing job roles
                        var existingJobRoles = await _context.JobRoles
                            .Where(jr => jr.WorkExperienceId == workExperience.Id)
                            .ToListAsync();

                        if (existingJobRoles.Any())
                        {
                            _context.JobRoles.RemoveRange(existingJobRoles);
                        }

                        // Add new job roles
                        var newJobRoles = updateData.WorkExperience.JobRoles
                            .Select(jr => new JobRole
                            {
                                JobTitle = jr.JobTitle,
                                CompanyName = jr.CompanyName,
                                StartDate = jr.StartDate,
                                EndDate = jr.EndDate,
                                JobDescription = jr.JobDescription,
                                WorkExperienceId = workExperience.Id
                            }).ToList();

                        if (newJobRoles.Any())
                        {
                            _context.JobRoles.AddRange(newJobRoles);
                        }
                    }
                }

                // Update skills
                if (updateData.Skills != null)
                {
                    // Get or create Skills record
                    var skills = await _context.Skills
                        .FirstOrDefaultAsync(s => s.UserId == userId);

                    if (skills == null)
                    {
                        skills = new Skills
                        {
                            UserId = userId
                        };
                        _context.Skills.Add(skills);
                        await _context.SaveChangesAsync(); // Save to get the ID
                    }

                    // Remove all existing skills for this Skills record
                    var existingSkills = await _context.Skill
                        .Where(s => s.SkillsId == skills.Id)
                        .ToListAsync();

                    if (existingSkills.Any())
                    {
                        _context.Skill.RemoveRange(existingSkills);
                    }

                    var newSkills = new List<Skill>();

                    // Add technical skills
                    if (updateData.Skills.TechnicalSkills != null)
                    {
                        newSkills.AddRange(updateData.Skills.TechnicalSkills
                            .Where(s => !string.IsNullOrEmpty(s))
                            .Select(s => new Skill
                            {
                                Name = s,
                                SkillType = "Technical",
                                SkillsId = skills.Id
                            }));
                    }

                    // Add soft skills
                    if (updateData.Skills.SoftSkills != null)
                    {
                        newSkills.AddRange(updateData.Skills.SoftSkills
                            .Where(s => !string.IsNullOrEmpty(s))
                            .Select(s => new Skill
                            {
                                Name = s,
                                SkillType = "Soft",
                                SkillsId = skills.Id
                            }));
                    }

                    // Add skills to learn
                    if (updateData.Skills.SkillsToLearn != null)
                    {
                        newSkills.AddRange(updateData.Skills.SkillsToLearn
                            .Where(s => !string.IsNullOrEmpty(s))
                            .Select(s => new Skill
                            {
                                Name = s,
                                SkillType = "ToLearn",
                                SkillsId = skills.Id
                            }));
                    }

                    if (newSkills.Any())
                    {
                        _context.Skill.AddRange(newSkills);
                    }
                }

                // Update career goals
                if (updateData.CareerGoals != null)
                {
                    var careerGoals = await _context.CareerGoals
                        .FirstOrDefaultAsync(cg => cg.UserId == userId);

                    if (careerGoals == null)
                    {
                        careerGoals = new CareerGoals
                        {
                            UserId = userId
                        };
                        _context.CareerGoals.Add(careerGoals);
                        await _context.SaveChangesAsync(); // Save to get the ID
                    }

                    if (!string.IsNullOrEmpty(updateData.CareerGoals.Timeframe))
                        careerGoals.Timeframe = updateData.CareerGoals.Timeframe;

                    if (!string.IsNullOrEmpty(updateData.CareerGoals.WorkPreference))
                        careerGoals.WorkPreference = updateData.CareerGoals.WorkPreference;

                    // Update goals
                    if (updateData.CareerGoals.Goals != null)
                    {
                        // Remove existing goals
                        var existingGoals = await _context.CareerGoal
                            .Where(cg => cg.CareerGoalsId == careerGoals.Id)
                            .ToListAsync();

                        if (existingGoals.Any())
                        {
                            _context.CareerGoal.RemoveRange(existingGoals);
                        }

                        // Add new goals
                        var newGoals = updateData.CareerGoals.Goals
                            .Where(g => !string.IsNullOrEmpty(g))
                            .Select(g => new CareerGoal
                            {
                                Goal = g,
                                CareerGoalsId = careerGoals.Id
                            }).ToList();

                        if (newGoals.Any())
                        {
                            _context.CareerGoal.AddRange(newGoals);
                        }
                    }

                    // Update preferred industries
                    if (updateData.CareerGoals.PreferredIndustries != null)
                    {
                        // Remove existing industries
                        var existingIndustries = await _context.PreferredIndustry
                            .Where(pi => pi.CareerGoalsId == careerGoals.Id)
                            .ToListAsync();

                        if (existingIndustries.Any())
                        {
                            _context.PreferredIndustry.RemoveRange(existingIndustries);
                        }

                        // Add new industries
                        var newIndustries = updateData.CareerGoals.PreferredIndustries
                            .Where(i => !string.IsNullOrEmpty(i))
                            .Select(i => new PreferredIndustry
                            {
                                Industry = i,
                                CareerGoalsId = careerGoals.Id
                            }).ToList();

                        if (newIndustries.Any())
                        {
                            _context.PreferredIndustry.AddRange(newIndustries);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new UpdatePersonalInfoResponseDto
                {
                    Success = true,
                    Message = "User information updated successfully"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error updating user info for user {UserId}", userId);
                return new UpdatePersonalInfoResponseDto
                {
                    Success = false,
                    Message = "An error occurred while updating user information"
                };
            }
        }
    }
}