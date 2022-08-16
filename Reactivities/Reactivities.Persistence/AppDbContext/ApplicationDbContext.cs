using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reactivities.Core.Models;
using Reactivities.Core.Models.Identity;
using Reactivities.Core.Models.Relations;

namespace Reactivities.Persistence.AppDbContext;

public class ApplicationDbContext : IdentityUserContext<ApplicationUser, Guid>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityAttendee> ActivityAttendees { get; set; }

    public DbSet<Photo> Photos { get; set; }

    public DbSet<UserFollowing> UserFollowings { get; set; }
    
    public DbSet<Comment> Comments { get; set; }     

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ActivityAttendee>(x =>
        {
            x.HasKey(aa => new { aa.ApplicationUserId, aa.ActivityId });

            x.HasOne(aa => aa.ApplicationUser)
                .WithMany(u => u.Activities)
                .HasForeignKey(aa => aa.ApplicationUserId);

            x.HasOne(aa => aa.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(aa => aa.ActivityId);
        });

        builder.Entity<Comment>()
            .HasOne(c => c.Activity)
            .WithMany(a => a.Comments)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserFollowing>(x =>
        {
            x.HasKey(x => new { x.ObserverId, x.TargetId });

            x.HasOne(uf => uf.Observer)
                .WithMany(u => u.Followings)
                .HasForeignKey(uf => uf.ObserverId);

            x.HasOne(uf => uf.Target)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.TargetId);
        });
    }
}