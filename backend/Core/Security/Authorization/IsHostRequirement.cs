using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Core.Security.Authorization
{
    public class IsHostRequirement : IAuthorizationRequirement { }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DatabaseContext _databaseContext;

        public IsHostRequirementHandler(IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _databaseContext = databaseContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var currentUsername = _httpContextAccessor.HttpContext.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var activityId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value.ToString());
            var activity = _databaseContext.Activities.FindAsync(activityId).Result;
            var host = activity.ActivityAttendees.FirstOrDefault(x => x.IsHost);

            if (host?.AppUser?.UserName == currentUsername) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}