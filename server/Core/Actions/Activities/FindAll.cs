using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.ViewModels;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.Activities
{
    public class FindAll
    {
        public class Query : IRequest<IReadOnlyList<ActivityViewModel>> { }

        public class Handler : IRequestHandler<Query, IReadOnlyList<ActivityViewModel>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<IReadOnlyList<ActivityViewModel>> Handle(
                Query query,
                CancellationToken cancellationToken)
            {
                var activities = await _dataContext.Activities.ToListAsync();
                return _mapper.Map<IReadOnlyList<Activity>, IReadOnlyList<ActivityViewModel>>(activities);
            }
        }
    }
}