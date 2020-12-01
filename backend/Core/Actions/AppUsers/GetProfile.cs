using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Contexts;
using Data.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            private readonly DatabaseContext _databaseContext;

            public Handler(DatabaseContext databaseContext) => _databaseContext = databaseContext;

            public async Task<ProfileViewModel> Handle(Query query, CancellationToken cancellationToken)
            {
                var appUser = await _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == query.Username);

                return new ProfileViewModel
                {
                    DisplayName = appUser.DisplayName,
                    Username = appUser.UserName,
                    PictureURL = appUser.Photos.FirstOrDefault(x => x.IsMain)?.URL,
                    Photos = appUser.Photos,
                    Bio = appUser.Bio,
                };
            }
        }
    }
}