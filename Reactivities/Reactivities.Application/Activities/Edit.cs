using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.Validators;
using Reactivities.Core.Models;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Activities;

public class Edit
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

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _dbContext.Activities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Activity.Id,
                cancellationToken: cancellationToken);
            
            activity = request.Activity;
            _dbContext.Entry(activity).State = EntityState.Modified;
            
            var result = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

            activity.Category = "f";
            return result 
                ? ServiceResult<Unit>.CreateSuccess(Unit.Value)
                : ServiceResult<Unit>.CreateFailure("Failed to update activity");
        }
    }
}