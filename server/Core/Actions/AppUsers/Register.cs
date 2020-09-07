using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces.Security;
using Core.Validators;
using Core.ViewModels;
using Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

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
                RuleFor(command => command.DisplayName).NotEmpty();
                RuleFor(command => command.Username).NotEmpty();
                RuleFor(command => command.Email).NotEmpty().EmailAddress();
                RuleFor(command => command.Password).ValidatePassword();
            }
        }

        public class Handler : IRequestHandler<Command, AppUserViewModel>
        {
            private readonly DataContext _dataContext;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJWTGenerator _jwtGenerator;

            public Handler(DataContext dataContext, UserManager<AppUser> userManager, IJWTGenerator jwtGenerator)
            {
                _dataContext = dataContext;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<AppUserViewModel> Handle(Command command, CancellationToken cancellationToken)
            {
                if (await _dataContext.Users.AnyAsync(appUser => appUser.Email == command.Email))
                    throw new RESTException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });

                if (await _dataContext.Users.AnyAsync(appUser => appUser.UserName == command.Username))
                    throw new RESTException(HttpStatusCode.BadRequest, new { Email = "Username already exists" });

                var appUser = new AppUser
                {
                    DisplayName = command.DisplayName,
                    Email = command.Email,
                    UserName = command.Username
                };

                var identityResult = await _userManager.CreateAsync(appUser, command.Password);

                if (identityResult.Succeeded) return new AppUserViewModel
                {
                    DisplayName = appUser.DisplayName,
                    Token = _jwtGenerator.CreateToken(appUser),
                    Username = appUser.UserName,
                    ProfilePictureURL = null
                };

                throw new Exception("Problem creating user");
            }
        }
    }
}