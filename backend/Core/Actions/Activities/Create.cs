using System;
using System.Threading;
using System.Threading.Tasks;
using Data.Contexts;
using MediatR;
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
            public DateTime DateTime { get; set; }
            public string City { get; set; }
            public string Venue { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext) => _databaseContext = databaseContext;

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var activity = new Activity
                {
                    Id = command.Id,
                    Title = command.Title,
                    Description = command.Description,
                    Category = command.Category,
                    DateTime = command.DateTime,
                    City = command.City,
                    Venue = command.Venue
                };

                _databaseContext.Activities.Add(activity);
                var activitySuccessfullyCreated = await _databaseContext.SaveChangesAsync() > 0;

                if (activitySuccessfullyCreated) return Unit.Value;

                throw new Exception("Problem creating new activity");
            }
        }
    }
}