using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagementSystem.EF.Entities;
using System.Net;

namespace TasksManagementSystem.EF.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUserEntity>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TaskEntity>()
                           .Property(p => p.Status)
                           .HasConversion<string>()
                           .HasMaxLength(50);


            // one to many Relations

            // User To Projects
            builder.Entity<ApplicationUserEntity>()
                  .HasMany(e => e.ProjectEntities)
                  .WithOne()
                  .HasForeignKey(f => f.CreatedById)
                  .OnDelete(DeleteBehavior.SetNull);

            // User To Tasks
            builder.Entity<ApplicationUserEntity>()
                  .HasMany(e => e.TaskEntities)
                  .WithOne()
                  .HasForeignKey(f => f.CreatedById)
                  .OnDelete(DeleteBehavior.SetNull);

            // User Assigned To Tasks
            builder.Entity<ApplicationUserEntity>()
                  .HasMany(e => e.TaskEntities)
                  .WithOne()
                  .HasForeignKey(f => f.AssignedToId)
                  .OnDelete(DeleteBehavior.SetNull);

            // User To Commments
            builder.Entity<ApplicationUserEntity>()
                  .HasMany(e => e.CommentEntities)
                  .WithOne()
                  .HasForeignKey(f => f.CreatedById)
                  .OnDelete(DeleteBehavior.Cascade);

            // Project To Tasks
            builder.Entity<ProjectEntity>()
              .HasMany(e => e.TaskEntities)
              .WithOne()
              .HasForeignKey(f => f.ProjectId)
              .OnDelete(DeleteBehavior.Cascade);

            // Task To Commments
            builder.Entity<TaskEntity>()
            .HasMany(e => e.CommentEntities)
            .WithOne()
            .HasForeignKey(f => f.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.SeedRoles();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.AddTimeStamps();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTimeStamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedAt = now;
                }

                ((BaseEntity)entity.Entity).UpdatedAt = now;
            }
        }
    }
}
