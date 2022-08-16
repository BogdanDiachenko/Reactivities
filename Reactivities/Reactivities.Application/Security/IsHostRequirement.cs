using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.Security;

public class IsHostRequirement : IAuthorizationRequirement
{
    
}

public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContext;

    public IsHostRequirementHandler(IHttpContextAccessor httpContext, ApplicationDbContext dbContext)
    {
        _httpContext = httpContext;
        _dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
    {
        var userId = Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty) ;

        if (userId == Guid.Empty)
        {
            return Task.CompletedTask;
        }

        var activityId = Guid.Parse(_httpContext.HttpContext?.Request.RouteValues
            .SingleOrDefault(x => x.Key == "id").Value?.ToString());

        var attendee = _dbContext.ActivityAttendees
            .AsNoTracking().SingleOrDefaultAsync(x => 
                x.ApplicationUserId == userId && x.ActivityId == activityId).Result;
        
        if (attendee == null)
        {
            return Task.CompletedTask;
        }

        if (attendee.IsHost)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}