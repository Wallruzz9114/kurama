using System.Reflection;
using Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Middleware.Contexts
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActivityAttendee>(builder => builder.HasKey(ua => new { ua.AppUserId, ua.ActivityId }));
            modelBuilder.Entity<ActivityAttendee>()
                .HasOne(aua => aua.AppUser)
                .WithMany(au => au.ActivityAttendees)
                .HasForeignKey(aua => aua.AppUserId);
            modelBuilder.Entity<ActivityAttendee>()
                .HasOne(aua => aua.Activity)
                .WithMany(au => au.ActivityAttendees)
                .HasForeignKey(aua => aua.ActivityId);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<SocialLink>(builder =>
            {
                builder.HasKey(link => new { link.SourceUserId, link.TargetUserId });

                builder.HasOne(link => link.SourceUser)
                    .WithMany(user => user.Favourites)
                    .HasForeignKey(l => l.SourceUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(link => link.TargetUser)
                    .WithMany(user => user.Followers)
                    .HasForeignKey(l => l.TargetUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}