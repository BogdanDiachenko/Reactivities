using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.Core.DTOs;
using Reactivities.Core.Helpers;
using Reactivities.Core.Helpers.Paging;
using Reactivities.Core.Models;
using Reactivities.Persistence.AppDbContext;
using StackExchange.Redis;

namespace Reactivities.BusinessLogic.Activities;

public class List
{
    public class Query : IRequest<ServiceResult<PagedList<ActivityDto>>>
    {
        public ActivityParams Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, ServiceResult<PagedList<ActivityDto>>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserAccessor _userAccessor;
        private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;

        public Handler(ApplicationDbContext dbContext, IUserAccessor userAccessor, IDistributedCache cache, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _userAccessor = userAccessor;
            _cache = cache;
            _cacheService = cacheService;
        }

        public async Task<ServiceResult<PagedList<ActivityDto>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Activity, bool>> filter = (_ => true);
            string recordId = "activities";
            if (request.Params.IsHost)
            {
                recordId = $"activities_host_{_userAccessor.GetUsername()}";
                filter = x => x.Attendees
                    .SingleOrDefault(x => x.IsHost).ApplicationUser.UserName == _userAccessor.GetUsername();
            }

            if (request.Params.IsGoing && !request.Params.IsHost)
            {
                recordId = $"activities_going_{_userAccessor.GetUsername()}";
                filter = x => x.Attendees
                    .Any(x => x.ApplicationUser.UserName == _userAccessor.GetUsername());
            }

            var activities = await _cacheService.GetRecordAsync<PagedList<ActivityDto>>(recordId);

            if (activities == default)
            {
                var query = _dbContext.Activities
                    .Where(a => a.Date >= request.Params.StartDate)
                    .OrderBy(a => a.Date)
                    .Include(a => a.Attendees)
                    .ThenInclude(aa => aa.ApplicationUser)
                    .ThenInclude(x => x.Photos)
                    .Include(x => x.Attendees)
                    .ThenInclude(x => x.ApplicationUser)
                    .ThenInclude(x => x.Followers)
                    .ThenInclude(x => x.Observer)
                    .Where(filter)
                    .Select(x => x.ToDto(_userAccessor.GetUsername()))
                    .AsQueryable();

                activities = await PagedList<ActivityDto>
                    .CreateAsync(query, request.Params.PageNumber, request.Params.PageSize);

                await _cacheService.SetRecordAsync(recordId, activities);
            }

            return ServiceResult<PagedList<ActivityDto>>.CreateSuccess(activities);
        }
    }
}