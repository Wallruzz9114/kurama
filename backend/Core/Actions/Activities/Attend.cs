using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces;
using Data.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Actions.Activities
{
    public class Attend
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DatabaseContext databaseContext, IUserAccessor userAccessor)
            {
                _databaseContext = databaseContext;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var activity = await _databaseContext.Activities.FindAsync(command.Id);

                if (activity == null)
                    throw new RESTException(HttpStatusCode.NotFound, new { Activity = "Not found" });

                var appUser = await _databaseContext.Users
                    .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());
                var alreadyAttending = await _databaseContext.ActivityAttendees
                    .AnyAsync(x => x.ActivityId == activity.Id && x.AppUserId == appUser.Id);

                if (alreadyAttending)
                    throw new RESTException(HttpStatusCode.NotFound, new { Attendance = "Already attending this activity" });

                var newAttendance = new ActivityAttendee
                {
                    Activity = activity,
                    AppUser = appUser,
                    IsHost = false,
                    DateJoined = DateTime.Now,
                };

                _databaseContext.ActivityAttendees.Add(newAttendance);

                var attendanceSuccessfullyCreated = await _databaseContext.SaveChangesAsync() > 0;

                if (attendanceSuccessfullyCreated) return Unit.Value;

                throw new Exception("Problem while trying to attend an activity");
            }
        }
    }
}