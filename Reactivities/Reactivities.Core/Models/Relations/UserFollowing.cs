using Reactivities.Core.Models.Identity;

namespace Reactivities.Core.Models.Relations;

public class UserFollowing
{
    public Guid ObserverId { get; set; }

    public ApplicationUser Observer { get; set; }

    public Guid TargetId { get; set; }

    public ApplicationUser Target { get; set; }
}