using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Reactivities.BusinessLogic.Photos;
using Reactivities.Core.Models;
using Reactivities.Core.Options;

namespace Reactivities.BusinessLogic.Accessors.PhotoAccessor;

public class PhotoAccessor : IPhotoAccessor
{
    private readonly Cloudinary _cloudinary;
    
    public PhotoAccessor(IOptions<CloudinaryOptions> options)
    {
        var account = new Account(
            options.Value.CloudName,
            options.Value.ApiKey,
            options.Value.ApiSecret
        );
        
        _cloudinary = new Cloudinary(account);
    }
    
    public async Task<PhotoUploadResult> AddPhoto(IFormFile file)
    {
        if (file.Length > 0)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill"),
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return new PhotoUploadResult
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString(),
            };
            
        }

        return null;
    }

    public async Task<string> DeletePhoto(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var deleteResult = await _cloudinary.DestroyAsync(deleteParams);

        return deleteResult.Result == "ok" ? deleteResult.Result : null;
    }
}