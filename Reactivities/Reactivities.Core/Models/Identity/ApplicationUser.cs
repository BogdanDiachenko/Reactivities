using Microsoft.AspNetCore.Identity;
using Reactivities.Core.Models.Base;
using Reactivities.Core.Models.Relations;

namespace Reactivities.Core.Models.Identity;

public class ApplicationUser : IdentityUser<Guid>, IBaseEntity<Guid>
{
    public string DisplayName { get; set; }

    public string Bio { get; set; }

    public ICollection<ActivityAttendee> Activities { get; set; } = new List<ActivityAttendee>();

    public ICollection<Photo> Photos { get; set; } = new List<Photo>();

    public ICollection<UserFollowing> Followings { get; set; } = new List<UserFollowing>();

    public ICollection<UserFollowing> Followers { get; set; } = new List<UserFollowing>();
}