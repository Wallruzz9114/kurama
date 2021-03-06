using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Data.Contexts;
using MediatR;

namespace Core.Actions.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;

            public Handler(DatabaseContext databaseContext) => _databaseContext = databaseContext;

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var activityToDelete = await _databaseContext.Activities.FindAsync(command.Id);

                if (activityToDelete == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { activity = "Not found" });

                _databaseContext.Remove(activityToDelete);

                var activitySuccessfullyDeleted = await _databaseContext.SaveChangesAsync() > 0;

                if (activitySuccessfullyDeleted) return Unit.Value;

                throw new Exception("Problem while deleting activity");
            }
        }
    }
}