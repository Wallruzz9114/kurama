using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Interfaces.Security;
using Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.Photos
{
    public class Upload
    {
        public class Command : IRequest<Photo>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Photo>
        {
            private readonly DataContext _dataContext;
            private readonly IAppUserService _appUserService;
            private readonly IPhotoService _photoService;

            public Handler(DataContext dataContext, IAppUserService appUserService, IPhotoService photoService)
            {
                _dataContext = dataContext;
                _appUserService = appUserService;
                _photoService = photoService;
            }

            public async Task<Photo> Handle(Command command, CancellationToken cancellationToken)
            {
                var photoUploadResult = _photoService.UploadPhoto(command.File);
                var appUser = await _dataContext.Users
                    .SingleOrDefaultAsync(appUser => appUser.UserName == _appUserService.GetCurrentAppUserUsername());

                var photo = new Photo
                {
                    Id = photoUploadResult.Id,
                    URL = photoUploadResult.URL
                };

                if (!appUser.Photos.Any(photo => photo.IsProfilePicture)) photo.IsProfilePicture = true;

                appUser.Photos.Add(photo);
                var photoUploaded = await _dataContext.SaveChangesAsync() > 0;

                if (photoUploaded) return photo;

                throw new Exception("Problem saving changes");
            }
        }
    }
}