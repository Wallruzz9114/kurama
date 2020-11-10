using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Data.Contexts;
using MediatR;
using Models;

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
            private readonly DatabaseContext _databaseContext;

            public Handler(DatabaseContext databaseContext) => _databaseContext = databaseContext;

            public async Task<Activity> Handle(Query query, CancellationToken cancellationToken)
            {
                throw new Exception("Computer says no");

                var activity = await _databaseContext.Activities.FindAsync(query.Id);

                if (activity == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { activity = "Not found" });

                return activity;
            }
        }
    }
}