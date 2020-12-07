using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Interfaces;
using Data.Contexts;
using Data.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Actions.Activities
{
    public class PaginatedActivities
    {
        public List<ActivityViewModel> Activities { get; set; }
        public int Count { get; set; }
    }

    public class GetAll
    {
        public class Query : IRequest<PaginatedActivities>
        {
            public int? Limit { get; set; }
            public int? Offset { get; set; }
            public bool IsGoing { get; set; }
            public bool IsHost { get; set; }
            public DateTime? StartDate { get; set; }

            public Query(int? limit, int? offset, bool isGoing, bool isHost, DateTime? startDate)
            {
                Limit = limit;
                Offset = offset;
                isGoing = IsGoing;
                isHost = IsHost;
                startDate = StartDate ?? DateTime.Now;
            }
        }

        public class Handler : IRequestHandler<Query, PaginatedActivities>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DatabaseContext databaseContext, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _databaseContext = databaseContext;
                _mapper = mapper;
            }

            public async Task<PaginatedActivities> Handle(Query query, CancellationToken cancellationToken)
            {
                var activitiesQuery = _databaseContext.Activities
                    .Where(x => x.Date >= query.StartDate)
                    .OrderBy(x => x.Date)
                    .AsQueryable();

                if (query.IsGoing && !query.IsHost)
                    activitiesQuery = activitiesQuery.Where(
                        x => x.ActivityAttendees.Any(a => a.AppUser.UserName == _userAccessor.GetCurrentUsername()));

                if (query.IsHost && !query.IsGoing)
                    activitiesQuery = activitiesQuery.Where(
                        x => x.ActivityAttendees.Any(a => a.AppUser.UserName == _userAccessor.GetCurrentUsername() && a.IsHost));

                var activities = await activitiesQuery.Skip(query.Offset ?? 0).Take(query.Limit ?? 3).ToListAsync();

                return new PaginatedActivities
                {
                    Activities = _mapper.Map<List<Activity>, List<ActivityViewModel>>(activities),
                    Count = activitiesQuery.Count(),
                };
            }
        }
    }
}