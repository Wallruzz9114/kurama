using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Data.Contexts;
using Data.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Actions.AppUsers
{
    public class GetRelationship
    {
        public class Query : IRequest<List<ProfileViewModel>>
        {
            public string Username { get; set; }
            public string Relationship { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<ProfileViewModel>>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DatabaseContext databaseContext, IUserAccessor userAccessor)
            {
                _databaseContext = databaseContext;
                _userAccessor = userAccessor;
            }

            public async Task<List<ProfileViewModel>> Handle(Query query, CancellationToken cancellationToken)
            {
                var usersRelationshipsQuery = _databaseContext.UserRelationships.AsQueryable();
                var userRelationships = new List<UserRelationship>();
                var userProfiles = new List<ProfileViewModel>();

                switch (query.Relationship)
                {
                    case "followers":
                        {
                            userRelationships = await usersRelationshipsQuery
                                .Where(x => x.UserFollowed.UserName == query.Username)
                                .ToListAsync();

                            foreach (var follower in userRelationships)
                                userProfiles.Add(await _userAccessor.GetProfile(follower.Follower.UserName));

                            break;
                        }
                    case "followings":
                        {
                            userRelationships = await usersRelationshipsQuery
                                .Where(x => x.Follower.UserName == query.Username)
                                .ToListAsync();

                            foreach (var following in userRelationships)
                                userProfiles.Add(await _userAccessor.GetProfile(following.UserFollowed.UserName));

                            break;
                        }
                }

                return userProfiles;
            }
        }
    }
}