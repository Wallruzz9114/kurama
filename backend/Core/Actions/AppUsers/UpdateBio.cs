using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Data.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Actions.AppUsers
{
    public class UpdateBio
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
                RuleFor(x => x.DisplayName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DatabaseContext databaseContext, IUserAccessor userAccessor)
            {
                _databaseContext = databaseContext;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var appUser = await _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                appUser.DisplayName = command.DisplayName ?? appUser.DisplayName;
                appUser.Bio = command.Bio ?? appUser.Bio;

                var updateIsSuccessful = await _databaseContext.SaveChangesAsync() > 0;

                if (updateIsSuccessful) return Unit.Value;

                throw new Exception("Problem updating bio");
            }
        }
    }
}