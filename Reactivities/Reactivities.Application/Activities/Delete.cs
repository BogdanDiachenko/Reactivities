using MediatR;
using Reactivities.Core.Models;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Activities;

public class Delete
{
    public class Command : IRequest<ServiceResult<Unit>>
    {
        public Guid Id { get; set; }
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
            var activity = await _dbContext.Activities.FindAsync(request.Id);
            
            _dbContext.Activities.Remove(activity);

            var result = await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            
            return result 
                ? ServiceResult<Unit>.CreateSuccess(Unit.Value) 
                : ServiceResult<Unit>.CreateFailure("Failed to delete the activity");
        }
    }
}