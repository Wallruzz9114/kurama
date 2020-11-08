using System;
using System.Threading;
using System.Threading.Tasks;
using Data.Contexts;
using MediatR;

namespace Core.Actions.Activities
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime? DateTime { get; set; }
            public string City { get; set; }
            public string Venue { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;

            public Handler(DatabaseContext databaseContext) => _databaseContext = databaseContext;

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var activityToEdit = await _databaseContext.Activities.FindAsync(command.Id);

                if (activityToEdit == null) throw new Exception("Could not find activity to edit");

                activityToEdit.Title = command.Title ?? activityToEdit.Title;
                activityToEdit.Description = command.Description ?? activityToEdit.Description;
                activityToEdit.Category = command.Category ?? activityToEdit.Category;
                activityToEdit.DateTime = command.DateTime ?? activityToEdit.DateTime;
                activityToEdit.City = command.City ?? activityToEdit.City;
                activityToEdit.Venue = command.Venue ?? activityToEdit.Venue;

                var activitySuccessfullyEdited = await _databaseContext.SaveChangesAsync() > 0;

                if (activitySuccessfullyEdited) return Unit.Value;

                throw new Exception("Problem while updating activity");
            }
        }
    }
}