using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.Core.DTOs;
using Reactivities.Core.Helpers;
using Reactivities.Core.Models;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Activities;

public class Details
{
    public class Query : IRequest<ServiceResult<ActivityDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, ServiceResult<ActivityDto>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserAccessor _userAccessor;

        public Handler(ApplicationDbContext dbContext, IUserAccessor userAccessor)
        {
            _dbContext = dbContext;
            _userAccessor = userAccessor;
        }

        public async  Task<ServiceResult<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _dbContext.Activities
                .Include(x => x.Attendees)
                .ThenInclude(a => a.ApplicationUser)
                .ThenInclude(x=> x.Followers)
                .ThenInclude(x => x.Observer)
                .Include(x => x.Attendees)
                .ThenInclude(x => x.ApplicationUser)
                .ThenInclude(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            return (activity == null
                ? null
                : ServiceResult<ActivityDto>.CreateSuccess(activity.ToDto(_userAccessor.GetUsername())))!;
        }
    }
}