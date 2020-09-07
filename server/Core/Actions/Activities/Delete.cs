using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using MediatR;
using Middleware.Contexts;

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
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext) => _dataContext = dataContext;

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities.FindAsync(command.Id);

                if (activity == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { activity = "Not Found" });

                _dataContext.Remove(activity);

                var activityRemoved = await _dataContext.SaveChangesAsync() > 0;
                if (activityRemoved) return Unit.Value;

                throw new Exception($"Problem removing activity: { activity.Id }");
            }
        }
    }
}