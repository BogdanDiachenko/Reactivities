using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.Core.Models;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Photos;

public class SetMain
{
    public class Command : IRequest<ServiceResult<Unit>>
    {
        public Guid Id { get; set; }
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
            var user = await _dbContext.Users.Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

            if (user == null) return null;

            var photo = user.Photos.FirstOrDefault(ph => ph.Id == request.Id);

            if (photo == null) return null;

            var currentMain = user.Photos.FirstOrDefault(ph => ph.IsMain);

            if (currentMain != null)
            {
                currentMain.IsMain = false;
                photo.IsMain = true;
            }

            return await _dbContext.SaveChangesAsync() > 0
                ? ServiceResult<Unit>.CreateSuccess(Unit.Value)
                : ServiceResult<Unit>.CreateFailure("Problem setting main problem");
        }
    }
}