using Data.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IImageAccessor
    {
        ImageUploadViewModel AddImage(IFormFile file);
        string DeleteImage(string publicId);
    }
}