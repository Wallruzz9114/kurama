using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces;
using Core.Interfaces.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.Photos
{
    public class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
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

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var appUser = await _dataContext.Users
                    .SingleOrDefaultAsync(appUser => appUser.UserName == _appUserService.GetCurrentAppUserUsername());
                var photo = appUser.Photos.FirstOrDefault(photo => photo.Id == command.Id);

                if (photo == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { Photo = "Not found" });

                if (photo.IsProfilePicture)
                    throw new RESTException(HttpStatusCode.BadRequest, new { Photo = "Can't delete main photo" });

                var photoDeleted = _photoService.DeletePhoto(command.Id);

                if (!photoDeleted) throw new Exception("Problem while deleting photo");

                appUser.Photos.Remove(photo);

                var photoRemoved = await _dataContext.SaveChangesAsync() > 0;
                if (photoRemoved) return Unit.Value;

                throw new Exception("Problem removing photo");
            }
        }
    }
}