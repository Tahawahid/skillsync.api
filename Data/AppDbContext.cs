using Microsoft.EntityFrameworkCore;
using skillsync.api.Models;

namespace skillsync.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PersonalDetails> PersonalDetails { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<WorkExperience> WorkExperience { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<CareerGoals> CareerGoals { get; set; }
        public DbSet<CareerGoal> CareerGoal { get; set; }
        public DbSet<PreferredIndustry> PreferredIndustry { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // PersonalDetails configurations
            modelBuilder.Entity<PersonalDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<PersonalDetails>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Education configurations
            modelBuilder.Entity<Education>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<Education>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Certification configurations
            modelBuilder.Entity<Certification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Education)
                    .WithMany(e => e.Certifications)
                    .HasForeignKey(e => e.EducationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // WorkExperience configurations
            modelBuilder.Entity<WorkExperience>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<WorkExperience>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // JobRole configurations
            modelBuilder.Entity<JobRole>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.WorkExperience)
                    .WithMany(e => e.JobRoles)
                    .HasForeignKey(e => e.WorkExperienceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Skills configurations
            modelBuilder.Entity<Skills>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<Skills>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Skill configurations
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Skills)
                    .WithMany()
                    .HasForeignKey(e => e.SkillsId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // CareerGoals configurations
            modelBuilder.Entity<CareerGoals>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<CareerGoals>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // CareerGoal configurations
            modelBuilder.Entity<CareerGoal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.CareerGoals)
                    .WithMany(e => e.Goals)
                    .HasForeignKey(e => e.CareerGoalsId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // PreferredIndustry configurations
            modelBuilder.Entity<PreferredIndustry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.CareerGoals)
                    .WithMany(e => e.PreferredIndustries)
                    .HasForeignKey(e => e.CareerGoalsId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}