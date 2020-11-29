using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Errors;
using Data.Contexts;
using Data.ViewModels;
using MediatR;
using Models;

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
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;

            public Handler(DatabaseContext databaseContext, IMapper mapper)
            {
                _databaseContext = databaseContext;
                _mapper = mapper;
            }

            public async Task<ActivityViewModel> Handle(Query query, CancellationToken cancellationToken)
            {
                var activity = await _databaseContext.Activities.FindAsync(query.Id);

                if (activity == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { activity = "Not found" });

                return _mapper.Map<Activity, ActivityViewModel>(activity);
            }
        }
    }
}