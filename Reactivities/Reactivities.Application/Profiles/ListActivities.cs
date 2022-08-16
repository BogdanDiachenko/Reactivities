using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Core.DTOs;
using Reactivities.Core.Helpers;
using Reactivities.Core.Models;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Profiles;

public class ListActivities
{
    public class Query : IRequest<ServiceResult<List<UserActivityDto>>>
    {
        public string Username { get; set; }

        public string Predicate { get; set; }
    }

    public class Handler : IRequestHandler<Query, ServiceResult<List<UserActivityDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _dbContext.ActivityAttendees
                .Include(x => x.Activity)
                .ThenInclude(x => x.Attendees)
                .ThenInclude(x => x.ApplicationUser)
                .Where(x => x.ApplicationUser.UserName == request.Username)
                .OrderBy(x => x.Activity.Date)
                .AsQueryable();

            query = request.Predicate switch
            {
                "past" => query.Where(a => a.Activity.Date <= DateTimeOffset.Now),
                "hosting" => query.Where(a => a.Activity.Attendees.SingleOrDefault(x => x.IsHost).ApplicationUser.UserName == request.Username),
                _ => query.Where(x => x.Activity.Date >= DateTimeOffset.Now)
            };

            var activities = await query.Select(x => x.ToUserActivityDto()).ToListAsync();

            return ServiceResult<List<UserActivityDto>>.CreateSuccess(activities);
        }
    }
}