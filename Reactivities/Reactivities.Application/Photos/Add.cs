using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.Accessors.PhotoAccessor;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.Core.Models;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Photos;

public class Add
{
    public class Command : IRequest<ServiceResult<Photo>>
    {
        public IFormFile File { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, ServiceResult<Photo>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserAccessor _userAccessor;

        public Handler(ApplicationDbContext dbContext, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
        {
            _dbContext = dbContext;
            _photoAccessor = photoAccessor;
            _userAccessor = userAccessor;
        }

        public async Task<ServiceResult<Photo>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername(), cancellationToken: cancellationToken);

            if (user == null) return null;

            var uploadResult = await _photoAccessor.AddPhoto(request.File);

            var photo = new Photo
            {
                Url = uploadResult.Url,
                PublicId = uploadResult.PublicId,
                IsMain = !user.Photos.Any(x => x.IsMain)
            };
            
            user.Photos.Add(photo);

            var result = await _dbContext.SaveChangesAsync() > 0;

            return result
                ? ServiceResult<Photo>.CreateSuccess(photo)
                : ServiceResult<Photo>.CreateFailure("Problem adding photo");
        }
    }
}