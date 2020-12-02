using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Contexts
{
    public class DatabaseContext : IdentityDbContext<AppUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserRelationship> UserRelationships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActivityAttendee>(x => x.HasKey(ua => new { ua.AppUserId, ua.ActivityId }));

            modelBuilder.Entity<AppUser>()
                .HasMany(p => p.Photos)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ActivityAttendee>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.ActivityAttendees)
                .HasForeignKey(u => u.AppUserId);

            modelBuilder.Entity<ActivityAttendee>()
                .HasOne(a => a.Activity)
                .WithMany(u => u.ActivityAttendees)
                .HasForeignKey(a => a.ActivityId);

            modelBuilder.Entity<UserRelationship>(mb =>
            {
                mb.HasKey(k => new { k.FollowerId, k.UserFollowedId });

                mb.HasOne(uwfi => uwfi.Follower)
                    .WithMany(f => f.UsersFollowed)
                    .HasForeignKey(fk => fk.FollowerId)
                    .OnDelete(DeleteBehavior.Restrict);

                mb.HasOne(utbf => utbf.UserFollowed)
                    .WithMany(f => f.Followers)
                    .HasForeignKey(fk => fk.UserFollowedId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}