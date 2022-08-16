using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.BusinessLogic.Validators;
using Reactivities.Core.Models;
using Reactivities.Core.Models.Relations;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Activities;

public class Create
{
    public class Command : IRequest<ServiceResult<Unit>>
    {
        public Activity Activity { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
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
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => 
                x.UserName == _userAccessor.GetUsername(), cancellationToken: cancellationToken);

            var attendee = new ActivityAttendee
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                IsHost = true
            };
            
            request.Activity.Attendees.Add(attendee);
            
            await _dbContext.Activities.AddAsync(request.Activity, cancellationToken);

            var result = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

            return result
               ? ServiceResult<Unit>.CreateSuccess(Unit.Value)
               : ServiceResult<Unit>.CreateFailure("Failed to create activity");
        }
    }
}