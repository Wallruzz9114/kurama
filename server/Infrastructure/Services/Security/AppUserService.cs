using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces.Security;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Infrastructure.Services.Security
{
    public class AppUserService : IAppUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;

        public AppUserService(
            DataContext dataContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProfileViewModel> GetProfile(string username)
        {
            var searchedUser = await _dataContext.Users
                    .SingleOrDefaultAsync(appUser => appUser.UserName == username);

            if (searchedUser == null)
                throw new RESTException(HttpStatusCode.NotFound, new { AppUser = "Can't find this user" });

            var appUser = await _dataContext.Users
                    .SingleOrDefaultAsync(appUser => appUser.UserName == GetCurrentAppUserUsername());

            var profile = new ProfileViewModel
            {
                DisplayName = searchedUser.DisplayName,
                Username = searchedUser.UserName,
                ProfileImageURL = searchedUser.Photos.FirstOrDefault(photo => photo.IsProfilePicture)?.URL,
                Bio = searchedUser.Bio,
                Photos = searchedUser.Photos,
                Followers = searchedUser.Followers.Count,
                Favourites = searchedUser.Favourites.Count,
                FollowedByAppUser = appUser.Favourites.Any(link => link.TargetUserId == searchedUser.Id)
            };

            return profile;
        }

        public string GetCurrentAppUserUsername() =>
            _httpContextAccessor.HttpContext.User?.Claims?
                .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}