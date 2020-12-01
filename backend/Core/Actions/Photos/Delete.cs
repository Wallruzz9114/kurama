using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces;
using Data.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            private readonly DatabaseContext _databaseContext;
            private readonly IUserAccessor _userAccessor;
            private readonly IImageAccessor _imageAccessor;

            public Handler(DatabaseContext databaseContext, IUserAccessor userAccessor, IImageAccessor imageAccessor)
            {
                _databaseContext = databaseContext;
                _userAccessor = userAccessor;
                _imageAccessor = imageAccessor;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var appUser = await _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                var photo = appUser.Photos.FirstOrDefault(x => x.Id == command.Id);

                if (photo == null) throw new RESTException(HttpStatusCode.NotFound, new { Photo = "Not found" });

                if (photo.IsMain)
                    throw new RESTException(HttpStatusCode.BadRequest, new { Photo = "You can't delete your main photo" });

                var deleteAttemptResult = _imageAccessor.DeleteImage(photo.Id);

                if (deleteAttemptResult == null) throw new Exception("Error: couldn't delete image");

                appUser.Photos.Remove(photo);

                var userSuccessfullyUpdated = await _databaseContext.SaveChangesAsync() > 0;

                if (userSuccessfullyUpdated) return Unit.Value;

                throw new Exception("Problem while deleting image");
            }
        }
    }
}