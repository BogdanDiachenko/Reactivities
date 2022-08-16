using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.Accessors.PhotoAccessor;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.Core.Models;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Photos;

public class Delete
{
    public class Command : IRequest<ServiceResult<Unit>>
    {
        public Guid Id { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, ServiceResult<Unit>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserAccessor _userAccessor;
        private readonly IPhotoAccessor _photoAccessor;

        public Handler(IPhotoAccessor photoAccessor, ApplicationDbContext dbContext, IUserAccessor userAccessor)
        {
            _photoAccessor = photoAccessor;
            _dbContext = dbContext;
            _userAccessor = userAccessor;
        }

        public async Task<ServiceResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

            if (user == null) return null;

            var photo = user.Photos.FirstOrDefault(p => p.Id == request.Id);

            if (photo == null) return null;
            
            if(photo.IsMain) return ServiceResult<Unit>.CreateFailure("You cannot delete your main photo.");

            await _photoAccessor.DeletePhoto(photo.PublicId);

            user.Photos.Remove(photo);
            
            return await _dbContext.SaveChangesAsync() > 0 
                ? ServiceResult<Unit>.CreateSuccess(Unit.Value) 
                : ServiceResult<Unit>.CreateFailure("Problem deleting photo.");
        }
    }
}