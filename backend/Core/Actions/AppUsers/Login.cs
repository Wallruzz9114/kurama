using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces;
using Data.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;

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
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, AppUserViewModel>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJWTGenerator _jwtGenerator;

            public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJWTGenerator jwtGenerator)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<AppUserViewModel> Handle(Query query, CancellationToken cancellationToken)
            {
                var appUser = await _userManager.FindByEmailAsync(query.Email);

                if (appUser == null) throw new RESTException(HttpStatusCode.Unauthorized);

                var loginAttempt = await _signInManager.CheckPasswordSignInAsync(appUser, query.Password, false);

                if (loginAttempt.Succeeded)
                {
                    return new AppUserViewModel
                    {
                        DisplayName = appUser.DisplayName,
                        Token = _jwtGenerator.CreateToken(appUser),
                        Username = appUser.UserName,
                        PictureURL = null,
                    };
                }

                throw new RESTException(HttpStatusCode.Unauthorized);
            }
        }
    }
}