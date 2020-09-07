using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Data;
using MediatR;
using Middleware.Contexts;

namespace Core.Actions.Activities
{
    public class GetOne
    {
        public class Query : IRequest<Activity>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Activity>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext) => _dataContext = dataContext;

            public async Task<Activity> Handle(Query query, CancellationToken cancellationToken)
            {
                var activityFromDatabase = await _dataContext.Activities.FindAsync(query.Id);

                if (activityFromDatabase == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { activityFromDatabase = "Not Found" });

                return activityFromDatabase;
            }
        }
    }
}