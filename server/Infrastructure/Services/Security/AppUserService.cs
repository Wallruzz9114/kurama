using System.Linq;
using System.Security.Claims;
using Core.Interfaces.Security;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Security
{
    public class AppUserService : IAppUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppUserService(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        public string GetCurrentAppUserUsername() =>
            _httpContextAccessor.HttpContext.User?.Claims?
                .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}