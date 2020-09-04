using System;
using System.Threading;
using System.Threading.Tasks;
using Data;
using MediatR;
using Middleware.Contexts;

namespace Core.Activities
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

            public async Task<Activity> Handle(Query query, CancellationToken cancellationToken) =>
                await _dataContext.Activities.FindAsync(query.Id);
        }
    }
}