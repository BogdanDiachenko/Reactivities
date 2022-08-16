using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.Core.Models;
using Reactivities.Core.Models.Relations;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Followers;

public class FollowToggle
{
    public class Command : IRequest<ServiceResult<Unit>>
    {
        public string TargetUsername { get; set; }
    }

    public class Handler : IRequestHandler<Command, ServiceResult<Unit>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserAccessor _userAccessor;

        public Handler(ApplicationDbContext dbContext, IUserAccessor userAccessor)
        {
            _dbContext = dbContext;
            _userAccessor = userAccessor;
        }

        public async Task<ServiceResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var observer = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            var target = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);

            if (target == null || observer == null) return null;

            var following = await _dbContext.UserFollowings.FindAsync(observer.Id, target.Id);

            if (following == null)
            {
                following = new UserFollowing
                {
                    Observer = observer,
                    Target = target,
                };

                _dbContext.UserFollowings.Add(following);
            }
            else
            {
                _dbContext.UserFollowings.Remove(following);
            }

            return await _dbContext.SaveChangesAsync() > 0
                ? ServiceResult<Unit>.CreateSuccess(Unit.Value)
                : ServiceResult<Unit>.CreateFailure("Error updating following");
        }
    }
}