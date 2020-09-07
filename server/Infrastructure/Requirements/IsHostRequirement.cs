using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Middleware.Contexts;

namespace Infrastructure.Requirements
{
    public class IsHostRequirement : IAuthorizationRequirement { }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;

        public IsHostRequirementHandler(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext handlerContext,
            IsHostRequirement requirement)
        {
            var currentAppUserUsername = _httpContextAccessor.HttpContext
                .User?.Claims?.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var activityId = Guid.Parse(_httpContextAccessor
                .HttpContext.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value.ToString());
            var activity = _dataContext.Activities.FindAsync(activityId).Result;
            var host = activity.ActivityAttendees.FirstOrDefault(attendee => attendee.IsHosting);

            if (host?.AppUser?.UserName == currentAppUserUsername) handlerContext.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}