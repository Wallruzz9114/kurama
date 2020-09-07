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
            private readonly IJWTGeneratorService _jWTGenerator;
            private readonly IAppUserService _appUserService;

            public Handler(
                UserManager<AppUser> userManager,
                IJWTGeneratorService jWTGenerator,
                IAppUserService appUserService)
            {
                _userManager = userManager;
                _jWTGenerator = jWTGenerator;
                _appUserService = appUserService;
            }

            public async Task<AppUserViewModel> Handle(Query query, CancellationToken cancellationToken)
            {
                var appUser = await _userManager.FindByNameAsync(_appUserService.GetCurrentAppUserUsername());
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