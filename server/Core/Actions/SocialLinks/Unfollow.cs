using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.SocialLinks
{
    public class Unfollow
    {
        public class Command : IRequest
        {
            public string Username { get; set; }
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
                var userToFollow = await _dataContext.Users
                    .SingleOrDefaultAsync(appUser => appUser.UserName == command.Username);

                if (userToFollow == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { UserToFollow = "Can't find this user" });

                var link = await _dataContext.SocialLinks.SingleOrDefaultAsync(
                    link => link.SourceUserId == appUser.Id && link.TargetUserId == userToFollow.Id
                );

                if (link == null)
                    throw new RESTException(
                        HttpStatusCode.BadRequest,
                        new { UserToFollow = "You have no link to this user" }
                    );

                _dataContext.SocialLinks.Remove(link);

                var unfollowed = await _dataContext.SaveChangesAsync() > 0;
                if (unfollowed) return Unit.Value;

                throw new Exception("Problem unfollowing user");
            }
        }
    }
}