using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using FluentValidation;
using MediatR;
using Middleware.Contexts;

namespace Core.Actions.Activities
{
    public class Update
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime? Date { get; set; }
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
                var activityFromDatabase = await _dataContext.Activities.FindAsync(command.Id);

                if (activityFromDatabase == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { activityFromDatabase = "Not Found" });

                activityFromDatabase.Title = command.Title ?? activityFromDatabase.Title;
                activityFromDatabase.Description = command.Description ?? activityFromDatabase.Description;
                activityFromDatabase.Category = command.Category ?? activityFromDatabase.Category;
                activityFromDatabase.Date = command.Date ?? activityFromDatabase.Date;
                activityFromDatabase.City = command.City ?? activityFromDatabase.City;
                activityFromDatabase.Venue = command.Venue ?? activityFromDatabase.Venue;

                var activityUpdated = await _dataContext.SaveChangesAsync() > 0;
                if (activityUpdated) return Unit.Value;

                throw new Exception($"Problem updating activity: { activityFromDatabase.Id }");
            }
        }
    }
}