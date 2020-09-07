using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.Security;
using Core.ViewModels;
using Data;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Actions.AppUsers
{
    public class GetCurrentAppUser
    {
        public class Query : IRequest<AppUserViewModel> { }

        public class Handler : IRequestHandler<Query, AppUserViewModel>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJWTGenerator _jWTGenerator;
            private readonly IAppUserAccessor _appUserAccessor;

            public Handler(
                UserManager<AppUser> userManager,
                IJWTGenerator jWTGenerator,
                IAppUserAccessor appUserAccessor)
            {
                _userManager = userManager;
                _jWTGenerator = jWTGenerator;
                _appUserAccessor = appUserAccessor;
            }

            public async Task<AppUserViewModel> Handle(Query query, CancellationToken cancellationToken)
            {
                var appUser = await _userManager.FindByNameAsync(_appUserAccessor.GetCurrentAppUserUsername());
                return new AppUserViewModel
                {
                    DisplayName = appUser.DisplayName,
                    Username = appUser.UserName,
                    Token = _jWTGenerator.CreateToken(appUser),
                    ProfilePictureURL = null
                };
            }
        }
    }
}