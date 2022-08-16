using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.Core.DTOs;
using Reactivities.Core.Helpers;
using Reactivities.Core.Models;
using Reactivities.Core.Models.Identity;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Followers;

public class List
{
    public class Query : IRequest<ServiceResult<List<Profile>>>
    {
        public bool RequestingFollowers { get; set; }

        public string Username { get; set; }
    }
    
    public class Handler : IRequestHandler<Query, ServiceResult<List<Profile>>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserAccessor _userAccessor;

        public Handler(ApplicationDbContext dbContext, IUserAccessor userAccessor)
        {
            _dbContext = dbContext;
            _userAccessor = userAccessor;
        }

        public async Task<ServiceResult<List<Profile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var profiles = request.RequestingFollowers
                ? await _dbContext.UserFollowings.Where(x => x.Target.UserName == request.Username)
                    .Include(x => x.Observer)
                    .Include(x => x.Target)
                    .Select(u => u.Observer.ToProfile(_userAccessor.GetUsername())).ToListAsync()
                : await _dbContext.UserFollowings.Where(x => x.Observer.UserName == request.Username)
                    .Select(u => u.Target.ToProfile(_userAccessor.GetUsername())).ToListAsync();
            
            return ServiceResult<List<Profile>>.CreateSuccess(profiles);
        }
    }
}