using System;
using System.Threading;
using System.Threading.Tasks;
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

            public async Task<Activity> Handle(Query query, CancellationToken cancellationToken) =>
                await _databaseContext.Activities.FindAsync(query.Id);
        }
    }
}