using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces.Security;
using Core.ViewModels;
using Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Actions.AppUsers
{
    public class Login
    {
        public class Query : IRequest<AppUserViewModel>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(query => query.Email).NotEmpty();
                RuleFor(query => query.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, AppUserViewModel>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJWTGenerator _jwtGenerator;

            public Handler(
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                IJWTGenerator jwtGenerator)
            {
                _signInManager = signInManager;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<AppUserViewModel> Handle(Query query, CancellationToken cancellationToken)
            {
                var appUser = await _userManager.FindByEmailAsync(query.Email);

                if (appUser == null)
                    throw new RESTException(HttpStatusCode.Unauthorized);

                var signInAttempt = await _signInManager.CheckPasswordSignInAsync(appUser, query.Password, false);

                if (signInAttempt.Succeeded)
                {
                    return new AppUserViewModel
                    {
                        DisplayName = appUser.DisplayName,
                        Token = _jwtGenerator.CreateToken(appUser),
                        Username = appUser.UserName,
                        ProfilePictureURL = null
                    };
                }

                throw new RESTException(HttpStatusCode.Unauthorized);
            }
        }
    }
}