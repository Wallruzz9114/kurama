using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Interfaces.Security;
using Core.Models;
using Core.ViewModels;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.Activities
{
    public class FindAll
    {
        public class Query : IRequest<PaginatedActivityResult>
        {
            public Query(int? limit, int? offset, bool isAttenting, bool isHosting, DateTime? startDate)
            {
                Limit = limit;
                Offset = offset;
                IsAttending = isAttenting;
                IsHosting = isHosting;
                StartDate = startDate ?? DateTime.Now;
            }

            public int? Limit { get; set; }
            public int? Offset { get; set; }
            public bool IsAttending { get; set; }
            public bool IsHosting { get; set; }
            public DateTime? StartDate { get; set; }
        }

        public class Handler : IRequestHandler<Query, PaginatedActivityResult>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IAppUserService _appUserService;

            public Handler(DataContext dataContext, IMapper mapper, IAppUserService appUserService)
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _appUserService = appUserService;
            }

            public async Task<PaginatedActivityResult> Handle(Query query, CancellationToken cancellationToken)
            {
                var queryable = _dataContext.Activities
                    .Where(activity => activity.Date >= query.StartDate)
                    .OrderBy(activity => activity.Date)
                    .AsQueryable();

                if (query.IsAttending && !query.IsHosting)
                {
                    queryable = queryable.Where(a => a.ActivityAttendees.Any(
                        at => at.AppUser.UserName == _appUserService.GetCurrentAppUserUsername())
                    );
                }

                if (query.IsHosting && !query.IsAttending)
                {
                    queryable = queryable.Where(
                        a => a.ActivityAttendees.Any(
                            at => at.AppUser.UserName == _appUserService.GetCurrentAppUserUsername() &&
                            at.IsHosting
                        )
                    );
                }

                var activities = await queryable
                    .Skip(query.Offset ?? 0)
                    .Take(query.Limit ?? 3)
                    .ToListAsync();

                return new PaginatedActivityResult
                {
                    Count = queryable.Count(),
                    Activities = _mapper
                        .Map<IReadOnlyList<Activity>, IReadOnlyList<ActivityViewModel>>(activities),
                };
            }
        }
    }
}