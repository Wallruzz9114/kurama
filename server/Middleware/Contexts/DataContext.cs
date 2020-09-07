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
        }
    }
}