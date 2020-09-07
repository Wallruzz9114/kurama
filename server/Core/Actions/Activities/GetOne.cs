using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Errors;
using Core.ViewModels;
using Data;
using MediatR;
using Middleware.Contexts;

namespace Core.Actions.Activities
{
    public class GetOne
    {
        public class Query : IRequest<ActivityViewModel>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ActivityViewModel>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<ActivityViewModel> Handle(Query query, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities.FindAsync(query.Id);

                if (activity == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { activity = "Not Found" });

                return _mapper.Map<Activity, ActivityViewModel>(activity);
            }
        }
    }
}