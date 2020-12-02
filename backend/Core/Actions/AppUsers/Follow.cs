using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces;
using Data.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Actions.AppUsers
{
    public class Follow
    {
        public class Command : IRequest
        {
            public string Username { get; set; }
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
                var userToFollow = await _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == command.Username);

                if (userToFollow == null) throw new RESTException(HttpStatusCode.NotFound, new { UserToFollow = "Not found" });

                var userRelationship = await _databaseContext.UserRelationships
                    .SingleOrDefaultAsync(x => x.FollowerId == appUser.Id && x.UserFollowedId == userToFollow.Id);

                if (userRelationship == null)
                {
                    userRelationship = new UserRelationship
                    {
                        Follower = appUser,
                        UserFollowed = userToFollow,
                    };

                    _databaseContext.UserRelationships.Add(userRelationship);
                }

                var attemptTofollowIsSuccessful = await _databaseContext.SaveChangesAsync() > 0;

                if (attemptTofollowIsSuccessful) return Unit.Value;

                throw new Exception("Problem while attempting to follow user");
            }
        }
    }
}