using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Data.Contexts;
using Data.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Actions.Activities
{
    public class ListAttendedActivities
    {
        public class Query : IRequest<List<ActivityAttendedViewModel>>
        {
            public string Username { get; set; }
            public string Predicate { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<ActivityAttendedViewModel>>
        {
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext) => _databaseContext = databaseContext;

            public async Task<List<ActivityAttendedViewModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                var appUser = await _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);

                if (appUser == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { AppUser = "Not found" });

                var queryable = appUser.ActivityAttendees.OrderBy(a => a.Activity.Date).AsQueryable();

                switch (request.Predicate)
                {
                    case "past":
                        queryable = queryable.Where(a => a.Activity.Date <= DateTime.Now);
                        break;
                    case "hosting":
                        queryable = queryable.Where(a => a.IsHost);
                        break;
                    default:
                        queryable = queryable.Where(a => a.Activity.Date >= DateTime.Now);
                        break;
                }

                var activityAttendees = queryable.ToList();
                var activitiesToReturn = new List<ActivityAttendedViewModel>();

                foreach (var activity in activityAttendees)
                {
                    var userActivity = new ActivityAttendedViewModel
                    {
                        Id = activity.Activity.Id,
                        Title = activity.Activity.Title,
                        Category = activity.Activity.Category,
                        Date = activity.Activity.Date
                    };

                    activitiesToReturn.Add(userActivity);
                }

                return activitiesToReturn;
            }
        }
    }
}