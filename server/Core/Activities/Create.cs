using System;
using System.Threading;
using System.Threading.Tasks;
using Data;
using FluentValidation;
using MediatR;
using Middleware.Contexts;

namespace Core.Activities
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
                RuleFor(command => command.Title).NotEmpty();
                RuleFor(command => command.Description).NotEmpty();
                RuleFor(command => command.Category).NotEmpty();
                RuleFor(command => command.Date).NotEmpty();
                RuleFor(command => command.City).NotEmpty();
                RuleFor(command => command.Venue).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext) => _dataContext = dataContext;

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

                _dataContext.Activities.Add(activity);

                var activityCreated = await _dataContext.SaveChangesAsync() > 0;

                if (activityCreated) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}