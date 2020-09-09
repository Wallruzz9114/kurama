using Core.Models;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IPhotoService
    {
        PhotoUploadResult UploadPhoto(IFormFile file);
        bool DeletePhoto(string id);
    }
}