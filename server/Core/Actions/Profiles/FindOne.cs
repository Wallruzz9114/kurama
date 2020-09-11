using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.Security;
using Data;
using MediatR;

namespace Core.Actions.Profiles
{
    public class FindOne
    {
        public class Query : IRequest<ProfileViewModel>
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, ProfileViewModel>
        {
            private readonly IAppUserService _appUserService;

            public Handler(IAppUserService appUserService) => _appUserService = appUserService;

            public async Task<ProfileViewModel> Handle(Query query, CancellationToken cancellationToken) =>
                await _appUserService.GetProfile(query.Username);
        }
    }
}