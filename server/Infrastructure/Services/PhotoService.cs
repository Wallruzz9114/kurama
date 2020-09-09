using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> settings)
        {
            var account = new Account(settings.Value.CloudName, settings.Value.APIKey, settings.Value.APISecret);
            _cloudinary = new Cloudinary(account);
        }

        public PhotoUploadResult UploadPhoto(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }

            if (uploadResult.Error != null)
                throw new Exception(uploadResult.Error.Message);

            return new PhotoUploadResult
            {
                Id = uploadResult.PublicId,
                URL = uploadResult.SecureUrl.AbsoluteUri
            };
        }

        public bool DeletePhoto(string id)
        {
            var deletionParams = new DeletionParams(id);
            var result = _cloudinary.Destroy(deletionParams);

            return result.Result == "ok" ? true : false;
        }
    }
}