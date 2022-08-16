using Reactivities.Core.Models.Base;
using Reactivities.Core.Models.Identity;
using Reactivities.Core.Models.Relations;

namespace Reactivities.Core.Models;

public class Activity : BaseEntity
{
    public string Title { get; set; }   
    
    public string Category { get; set; }
    
    public string Description { get; set; }

    public string City { get; set; }

    public string Venue { get; set; }   
    
    public bool IsCancelled { get; set; }
    
    public DateTimeOffset Date { get; set; }

    public ICollection<ActivityAttendee> Attendees { get; set; } = new List<ActivityAttendee>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}