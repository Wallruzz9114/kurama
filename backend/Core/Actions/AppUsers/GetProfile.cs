using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Data.ViewModels;
using MediatR;

namespace Core.Actions.AppUsers
{
    public class GetProfile
    {
        public class Query : IRequest<ProfileViewModel>
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, ProfileViewModel>
        {
            private readonly IUserAccessor _userAccessor;

            public Handler(IUserAccessor userAccessor) => _userAccessor = userAccessor;

            public async Task<ProfileViewModel> Handle(Query query, CancellationToken cancellationToken) =>
                await _userAccessor.GetProfile(query.Username);
        }
    }
}