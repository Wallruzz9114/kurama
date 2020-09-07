using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Actions.Activities
{
    public class CancelAttendance
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;
            private readonly IAppUserService _appUserService;

            public Handler(DataContext dataContext, IAppUserService appUserService)
            {
                _dataContext = dataContext;
                _appUserService = appUserService;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities.FindAsync(command.Id);

                if (activity == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { Activity = "Could not find activity" });

                var appUser = await _dataContext.Users
                    .SingleOrDefaultAsync(appUser =>
                        appUser.UserName == _appUserService.GetCurrentAppUserUsername()
                    );
                var activityAttendee = await _dataContext.ActivityAttendees
                    .SingleOrDefaultAsync(aa => aa.ActivityId == activity.Id && aa.AppUserId == appUser.Id);

                if (activityAttendee == null) return Unit.Value;

                if (activityAttendee.IsHosting)
                    throw new RESTException(
                        HttpStatusCode.BadRequest,
                        new { activityAttendee = "You can't remove yourself as host" }
                    );

                _dataContext.ActivityAttendees.Remove(activityAttendee);

                var attendanceCancelled = await _dataContext.SaveChangesAsync() > 0;
                if (attendanceCancelled) return Unit.Value;

                throw new Exception("Problem cancelling attendance");
            }
        }
    }
}