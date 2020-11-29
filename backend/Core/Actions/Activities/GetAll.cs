using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data.Contexts;
using Data.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Actions.Activities
{
    public class GetAll
    {
        public class Query : IRequest<List<ActivityViewModel>> { }

        public class Handler : IRequestHandler<Query, List<ActivityViewModel>>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;

            public Handler(DatabaseContext databaseContext, IMapper mapper)
            {
                _databaseContext = databaseContext;
                _mapper = mapper;
            }

            public async Task<List<ActivityViewModel>> Handle(Query query, CancellationToken cancellationToken)
            {
                var activities = await _databaseContext.Activities.ToListAsync();
                return _mapper.Map<List<Activity>, List<ActivityViewModel>>(activities);
            }
        }
    }
}