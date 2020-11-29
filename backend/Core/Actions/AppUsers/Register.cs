using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Extensions;
using Core.Interfaces;
using Data.Contexts;
using Data.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Actions.AppUsers
{
    public class Register
    {
        public class Command : IRequest<AppUserViewModel>
        {
            public string DisplayName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }

        public class Handler : IRequestHandler<Command, AppUserViewModel>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJWTGenerator _jwtGenerator;

            public Handler(DatabaseContext databaseContext, UserManager<AppUser> userManager, IJWTGenerator jwtGenerator)
            {
                _databaseContext = databaseContext;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<AppUserViewModel> Handle(Command command, CancellationToken cancellationToken)
            {
                if (await _databaseContext.Users.AnyAsync(x => x.Email == command.Email))
                    throw new RESTException(HttpStatusCode.BadRequest, new { Email = "Email already in use." });

                if (await _databaseContext.Users.AnyAsync(x => x.UserName == command.Username))
                    throw new RESTException(HttpStatusCode.BadRequest, new { Username = "Username already in use." });

                var appUser = new AppUser
                {
                    DisplayName = command.DisplayName,
                    Email = command.Email,
                    UserName = command.Username,
                };

                var appUserCreationAttempt = await _userManager.CreateAsync(appUser, command.Password);

                if (appUserCreationAttempt.Succeeded)
                    return new AppUserViewModel
                    {
                        DisplayName = appUser.DisplayName,
                        Token = _jwtGenerator.CreateToken(appUser),
                        Username = appUser.UserName,
                        PictureURL = null,
                    };

                throw new Exception("Problem while registering user");
            }
        }
    }
}