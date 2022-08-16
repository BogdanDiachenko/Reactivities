using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.Core.DTOs;
using Reactivities.Core.Helpers;
using Reactivities.Core.Models;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Profiles;

public class Details
{
    public class Query : IRequest<ServiceResult<Profile>>
    {
        public string Username { get; set; }
    }
    
    public class Handler : IRequestHandler<Query, ServiceResult<Profile>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserAccessor _userAccessor;

        public Handler(ApplicationDbContext dbContext, IUserAccessor userAccessor)
        {
            _dbContext = dbContext;
            _userAccessor = userAccessor;
        }

        public async Task<ServiceResult<Profile>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Include(u => u.Photos)
                .Include(u => u.Followers)
                .ThenInclude(x => x.Observer)
                .Include(u => u.Followings)
                .SingleOrDefaultAsync(u => u.UserName == request.Username);
            
            return user == null 
                ? ServiceResult<Profile>.CreateFailure("There is no user with that username")
                : ServiceResult<Profile>.CreateSuccess(user.ToProfile(_userAccessor.GetUsername()));
        }
    }
}