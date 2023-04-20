using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipApp.Core.Database
{
    public class AppDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        // public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<InternGroup> InternGroups { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Conversation> Conversations { get; set; } = null!;
        public virtual DbSet<Evaluation> Evaluations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Instructor>().ToTable("Instructors");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Recruiter>().ToTable("Recruiters");

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r!.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u!.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<StudentSkill>(entity =>
            {
                entity.HasOne(ss => ss.Student)
                    .WithMany(stu => stu!.StudentSkills)
                    .HasForeignKey(ss => ss.StudentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ss => ss.Skill)
                    .WithMany(skl => skl!.StudentSkills)
                    .HasForeignKey(ss => ss.SkillId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasKey(ss => new { ss.SkillId, ss.StudentId });
            });


            modelBuilder.Entity<StudentJob>(entity =>
            {
                entity.HasOne(sj => sj.Student)
                    .WithMany(stu => stu!.StudentJobs)
                    .HasForeignKey(sj => sj.StudentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sj => sj.Job)
                    .WithMany(job => job.StudentJobs)
                    .HasForeignKey(sj => sj.JobId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasKey(sj => new { sj.JobId, sj.StudentId });
            });


            modelBuilder.Entity<JobSkill>(entity =>
            {
                entity.HasOne(js => js.Job)
                    .WithMany(job => job!.JobSkills)
                    .HasForeignKey(js => js.JobId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(js => js.Skill)
                    .WithMany(skl => skl!.JobSkills)
                    .HasForeignKey(js => js.SkillId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasKey(js => new { js.SkillId, js.JobId });
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(msg => msg.User)
                    .WithMany(usr => usr!.Messages)
                    .HasForeignKey(msg => msg.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
