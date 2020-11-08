using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Data.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Actions.Activities
{
    public class GetAll
    {
        public class Query : IRequest<List<Activity>> { }

        public class Handler : IRequestHandler<Query, List<Activity>>
        {
            private readonly DatabaseContext _databaseContext;

            public Handler(DatabaseContext databaseContext) => _databaseContext = databaseContext;

            public async Task<List<Activity>> Handle(Query query, CancellationToken cancellationToken) =>
                await _databaseContext.Activities.ToListAsync();
        }
    }
}