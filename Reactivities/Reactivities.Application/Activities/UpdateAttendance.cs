using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.Core.Models;
using Reactivities.Core.Models.Relations;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Activities;

public class UpdateAttendance
{
    public class Command : IRequest<ServiceResult<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, ServiceResult<Unit>>
    {
        private ApplicationDbContext _dbContext;
        private readonly IUserAccessor _userAccessor;
        
        public Handler(ApplicationDbContext dbContext, IUserAccessor userAccessor)
        {
            _dbContext = dbContext;
            _userAccessor = userAccessor;
        }

        public async Task<ServiceResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _dbContext.Activities
                .Include(a => a.Attendees)
                .ThenInclude(aa => aa.ApplicationUser)
                .SingleOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (activity == null)
            {
                return null;
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => 
                u.UserName == _userAccessor.GetUsername(), cancellationToken);

            if (user == null)
            {
                return null;
            }
            
            var hostUsername = activity.Attendees.FirstOrDefault(x => x.IsHost)?.ApplicationUser.UserName;

            var attendance = activity.Attendees.FirstOrDefault(x => x.ApplicationUser.UserName == user.UserName);
            
            if (attendance != null && hostUsername == user.UserName)
            {
                activity.IsCancelled = !activity.IsCancelled;
            }

            if (attendance != null && hostUsername != user.UserName)
            {
                activity.Attendees.Remove(attendance);
            }
            
            if (attendance == null)
            {
                attendance = new ActivityAttendee
                {
                    ApplicationUser = user,
                    Activity = activity,
                    IsHost = false
                };

                activity.Attendees.Add(attendance);
            }

            var result = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

            return result
                ? ServiceResult<Unit>.CreateSuccess(Unit.Value)
                : ServiceResult<Unit>.CreateFailure("Problem updating attendance");
        }
    }
}