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
    public class SetAsMain
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DatabaseContext databaseContext, IUserAccessor userAccessor)
            {
                _databaseContext = databaseContext;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var appUser = await _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());
                var photo = appUser.Photos.FirstOrDefault(x => x.Id == command.Id);

                if (photo == null) throw new RESTException(HttpStatusCode.NotFound, new { Photo = "not found" });

                var currentMainPhoto = appUser.Photos.FirstOrDefault(x => x.IsMain);

                currentMainPhoto.IsMain = false;
                photo.IsMain = true;

                var updateIsSuccessful = await _databaseContext.SaveChangesAsync() > 0;

                if (updateIsSuccessful) return Unit.Value;

                throw new Exception("Problem setting main photo");
            }
        }
    }
}