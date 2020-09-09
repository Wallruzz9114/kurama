using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.Photos
{
    public class SetProfilePicture
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;
            private readonly IAppUserService _appUserService;

            public Handler(DataContext dataContext, IAppUserService appUserService)
            {
                _dataContext = dataContext;
                _appUserService = appUserService;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var appUser = await _dataContext.Users
                    .SingleOrDefaultAsync(appUser => appUser.UserName == _appUserService.GetCurrentAppUserUsername());
                var photo = appUser.Photos.FirstOrDefault(photo => photo.Id == command.Id);

                if (photo == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { Photo = "Not found" });

                var currentProfilePicture = appUser.Photos.FirstOrDefault(photo => photo.IsProfilePicture);

                currentProfilePicture.IsProfilePicture = false;
                photo.IsProfilePicture = true;

                var newProfilePictureSet = await _dataContext.SaveChangesAsync() > 0;
                if (newProfilePictureSet) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}