using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Data.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Actions.Activities
{
    public class Create
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime Date { get; set; }
            public string City { get; set; }
            public string Venue { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Category).NotEmpty();
                RuleFor(x => x.Date).NotEmpty();
                RuleFor(x => x.City).NotEmpty();
                RuleFor(x => x.Venue).NotEmpty();
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
                var activity = new Activity
                {
                    Id = command.Id,
                    Title = command.Title,
                    Description = command.Description,
                    Category = command.Category,
                    Date = command.Date,
                    City = command.City,
                    Venue = command.Venue
                };
                var appUser = await _databaseContext.Users.SingleOrDefaultAsync(
                    x => x.UserName == _userAccessor.GetCurrentUsername()
                );
                var activityAttendee = new ActivityAttendee
                {
                    AppUser = appUser,
                    Activity = activity,
                    IsHost = true,
                    DateJoined = DateTime.Now,
                };

                _databaseContext.Activities.Add(activity);
                _databaseContext.ActivityAttendees.Add(activityAttendee);

                var dataSuccessfullySaved = await _databaseContext.SaveChangesAsync() > 0;

                if (dataSuccessfullySaved) return Unit.Value;

                throw new Exception("Problem creating new activity");
            }
        }
    }
}