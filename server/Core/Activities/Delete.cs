using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using MediatR;
using Middleware.Contexts;

namespace Core.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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

                _dataContext.Remove(activityFromDatabase);

                var activityRemoved = await _dataContext.SaveChangesAsync() > 0;
                if (activityRemoved) return Unit.Value;

                throw new Exception($"Problem removing activity: { activityFromDatabase.Id }");
            }
        }
    }
}