using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Data.Contexts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;

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
            private readonly DatabaseContext _databaseContext;
            private readonly IUserAccessor _userAccessor;
            private readonly IImageAccessor _imageAccessor;

            public Handler(DatabaseContext databaseContext, IUserAccessor userAccessor, IImageAccessor imageAccessor)
            {
                _databaseContext = databaseContext;
                _userAccessor = userAccessor;
                _imageAccessor = imageAccessor;
            }

            public async Task<Photo> Handle(Command command, CancellationToken cancellationToken)
            {
                var imageUploadViewModel = _imageAccessor.AddImage(command.File);
                var appUser = await _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());
                var photo = new Photo
                {
                    Id = imageUploadViewModel.PublicId,
                    URL = imageUploadViewModel.URL,
                };

                if (!appUser.Photos.Any(x => x.IsMain)) photo.IsMain = true;

                appUser.Photos.Add(photo);

                var photoSuccessfullyAdded = await _databaseContext.SaveChangesAsync() > 0;

                if (photoSuccessfullyAdded) return photo;

                throw new Exception("Problem uploading photo");
            }
        }
    }
}