using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces.Security;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.SocialLinks
{
    public class Follow
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
                var currentUser = await _dataContext.Users
                    .SingleOrDefaultAsync(cu => cu.UserName == _appUserService.GetCurrentAppUserUsername());
                var userToFollow = await _dataContext.Users
                    .SingleOrDefaultAsync(utf => utf.UserName == command.Username);

                if (userToFollow == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { UserToFollow = "Can't find this user" });

                var socialLinkWithUser = await _dataContext.SocialLinks.SingleOrDefaultAsync(
                    link => link.SourceUserId == currentUser.Id && link.TargetUserId == userToFollow.Id
                );

                if (socialLinkWithUser != null)
                    throw new RESTException(
                        HttpStatusCode.BadRequest,
                        new { UserToFollow = "You're already following this user" }
                    );

                if (socialLinkWithUser == null)
                {
                    socialLinkWithUser = new SocialLink
                    {
                        SourceUser = currentUser,
                        TargetUser = userToFollow
                    };

                    _dataContext.SocialLinks.Add(socialLinkWithUser);
                }

                var socialLinkIsEstablished = await _dataContext.SaveChangesAsync() > 0;
                if (socialLinkIsEstablished) return Unit.Value;

                throw new Exception("Problem following user");
            }
        }
    }
}