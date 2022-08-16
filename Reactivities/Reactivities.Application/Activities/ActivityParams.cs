using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Reactivities.Core.Helpers.Paging;

namespace Reactivities.BusinessLogic.Activities;

public class ActivityParams : PagingParams
{
    public bool IsGoing { get; set; }   
    
    public bool IsHost { get; set; }
    
    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;
}