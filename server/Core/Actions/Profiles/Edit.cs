using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.Security;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.Profiles
{
    public class Edit
    {
        public class Command : IRequest
        {
            public string DisplayName { get; set; }
            public string Bio { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(command => command.DisplayName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;
            private readonly IAppUserService _appUserService;

            public Handler(DataContext dataContext, IAppUserService appUserService)
            {
                _dataContext = dataContext;
                _appUserService = appUserService;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var appUser = await _dataContext.Users
                    .SingleOrDefaultAsync(appUser =>
                        appUser.UserName == _appUserService.GetCurrentAppUserUsername()
                    );

                appUser.DisplayName = command.DisplayName ?? appUser.DisplayName;
                appUser.Bio = command.Bio ?? appUser.Bio;

                var profileUpdated = await _dataContext.SaveChangesAsync() > 0;
                if (profileUpdated) return Unit.Value;

                throw new Exception("Problem updating profile");
            }
        }
    }
}