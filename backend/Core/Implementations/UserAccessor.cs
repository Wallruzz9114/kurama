using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Errors;
using Core.Interfaces;
using Data.Contexts;
using Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Core.Implementations
{
    public class UserAccessor : IUserAccessor
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(DatabaseContext databaseContext, IHttpContextAccessor httpContextAccessor)
        {
            _databaseContext = databaseContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUsername() =>
            _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        public async Task<ProfileViewModel> GetProfile(string username)
        {
            var requestedUser = await _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == username);

            if (requestedUser == null)
                throw new RESTException(HttpStatusCode.NotFound, new { RequestedUser = "Not found" });

            var currentUser = await _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == GetCurrentUsername());
            var profileViewModel = new ProfileViewModel
            {
                DisplayName = requestedUser.DisplayName,
                Username = requestedUser.UserName,
                PictureURL = requestedUser.Photos.FirstOrDefault(x => x.IsMain)?.URL,
                Photos = requestedUser.Photos,
                Bio = requestedUser.Bio,
                FollowersCount = requestedUser.Followers.Count(),
                FollowingsCount = requestedUser.UsersFollowed.Count(),
            };

            if (currentUser.UsersFollowed.Any(x => x.UserFollowedId == requestedUser.Id))
                profileViewModel.CurrentUserIsFollowing = true;

            if (currentUser.Followers.Any(x => x.FollowerId == currentUser.Id))
                profileViewModel.IsAFollowerOfCurrentUser = true;

            return profileViewModel;
        }
    }
}