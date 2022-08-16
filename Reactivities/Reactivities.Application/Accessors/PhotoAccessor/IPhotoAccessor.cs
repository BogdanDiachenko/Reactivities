using Microsoft.AspNetCore.Http;
using Reactivities.BusinessLogic.Photos;
using Reactivities.Core.Models;

namespace Reactivities.BusinessLogic.Accessors.PhotoAccessor;

public interface IPhotoAccessor
{
    Task<PhotoUploadResult> AddPhoto(IFormFile file);

    Task<string> DeletePhoto(string publicId);
}