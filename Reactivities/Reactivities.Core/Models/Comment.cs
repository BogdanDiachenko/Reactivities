using Reactivities.Core.Models.Base;
using Reactivities.Core.Models.Identity;

namespace Reactivities.Core.Models;

public class Comment : BaseEntity
{
    public string Body { get; set; }

    public ApplicationUser Author { get; set; }

    public Activity Activity { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}