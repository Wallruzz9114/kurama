using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.Activities
{
    public class ListAll
    {
        public class Query : IRequest<IReadOnlyList<Activity>> { }

        public class Handler : IRequestHandler<Query, IReadOnlyList<Activity>>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext) => _dataContext = dataContext;

            public async Task<IReadOnlyList<Activity>> Handle(Query query, CancellationToken cancellationToken) =>
                await _dataContext.Activities.ToListAsync();
        }
    }
}