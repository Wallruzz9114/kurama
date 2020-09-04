using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Middleware.Contexts;

namespace Core.Activities
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

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext) => _dataContext = dataContext;

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                // Get activity from database
                var activityFromDatabase = await _dataContext.Activities.FindAsync(command.Id);
                // Throw exception if activity can't be found
                if (activityFromDatabase == null)
                    throw new Exception($"Could not find activity: { activityFromDatabase.Id }");

                // Update activity properties
                activityFromDatabase.Title = command.Title ?? activityFromDatabase.Title;
                activityFromDatabase.Description = command.Description ?? activityFromDatabase.Description;
                activityFromDatabase.Category = command.Category ?? activityFromDatabase.Category;
                activityFromDatabase.Date = command.Date ?? activityFromDatabase.Date;
                activityFromDatabase.City = command.City ?? activityFromDatabase.City;
                activityFromDatabase.Venue = command.Venue ?? activityFromDatabase.Venue;

                // Save changes and handle consequences
                var activityUpdated = await _dataContext.SaveChangesAsync() > 0;
                if (activityUpdated) return Unit.Value;

                throw new Exception($"Problem updating activity: { activityFromDatabase.Id }");
            }
        }
    }
}