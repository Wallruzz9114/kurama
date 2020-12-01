using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.Interfaces;
using Core.Security.Settings;
using Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Core.Implementations
{
    public class ImageAccessor : IImageAccessor
    {
        private readonly Cloudinary _cloudinary;

        public ImageAccessor(IOptions<CloudinarySettings> cloudinaryConfiguration)
        {
            var account = new Account(
                cloudinaryConfiguration.Value.CloudName,
                cloudinaryConfiguration.Value.APIKey,
                cloudinaryConfiguration.Value.APISecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public ImageUploadViewModel AddImage(IFormFile file)
        {
            var imageUploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    };

                    imageUploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            if (imageUploadResult.Error != null) throw new Exception(imageUploadResult.Error.Message);

            return new ImageUploadViewModel
            {
                PublicId = imageUploadResult.PublicId,
                URL = imageUploadResult.SecureUrl.AbsoluteUri
            };
        }

        public string DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var deletionResult = _cloudinary.Destroy(deleteParams);

            return deletionResult.Result == "ok" ? deletionResult.Result : null;
        }
    }
}