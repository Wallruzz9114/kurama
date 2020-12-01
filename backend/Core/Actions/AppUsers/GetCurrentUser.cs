using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Data.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;

namespace Core.Actions.AppUsers
{
    public class GetCurrentUser
    {
        public class Query : IRequest<AppUserViewModel> { }

        public class Handler : IRequestHandler<Query, AppUserViewModel>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJWTGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;

            public Handler(UserManager<AppUser> userManager, IJWTGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
            }

            public async Task<AppUserViewModel> Handle(Query query, CancellationToken cancellationToken)
            {
                var appUser = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                return new AppUserViewModel
                {
                    DisplayName = appUser.DisplayName,
                    Username = appUser.UserName,
                    Token = _jwtGenerator.CreateToken(appUser),
                    PictureURL = appUser.Photos.FirstOrDefault(x => x.IsMain)?.URL,
                };
            }
        }
    }
}