using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces;
using Data.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Actions.Activities
{
    public class Reject
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
                var attendance = await _databaseContext.ActivityAttendees
                    .SingleOrDefaultAsync(x => x.ActivityId == activity.Id && x.AppUserId == appUser.Id);

                if (attendance == null) return Unit.Value;

                if (attendance.IsHost)
                    throw new RESTException(
                        HttpStatusCode.NotFound,
                        new { Attendance = "Can't stop attending an activity you're hosting" }
                    );

                _databaseContext.ActivityAttendees.Remove(attendance);

                var attendanceSuccessfullyRemoved = await _databaseContext.SaveChangesAsync() > 0;

                if (attendanceSuccessfullyRemoved) return Unit.Value;

                throw new Exception("Problem while rejecting attendance");
            }
        }
    }
}