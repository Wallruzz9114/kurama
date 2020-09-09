using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.Photos
{
    public class GetProfile
    {
        public class Query : IRequest<ProfileViewModel>
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, ProfileViewModel>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext) => _dataContext = dataContext;

            public async Task<ProfileViewModel> Handle(Query query, CancellationToken cancellationToken)
            {
                var appUser = await _dataContext.Users.SingleOrDefaultAsync(au => au.UserName == query.Username);
                return new ProfileViewModel
                {
                    DisplayName = appUser.DisplayName,
                    Username = appUser.UserName,
                    ProfileImageURL = appUser.Photos.FirstOrDefault(photo => photo.IsProfilePicture)?.URL,
                    Bio = appUser.Bio,
                    Photos = appUser.Photos
                };
            }
        }
    }
}