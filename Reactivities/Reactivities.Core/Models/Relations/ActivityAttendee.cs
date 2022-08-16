using Reactivities.Core.Models.Identity;

namespace Reactivities.Core.Models.Relations;

public class ActivityAttendee
{
    public Guid ApplicationUserId { get; set; }
    
    public ApplicationUser ApplicationUser { get; set; }
    
    public Guid ActivityId { get; set; }
    
    public Activity Activity { get; set; }
    
    public bool IsHost { get; set; }
}