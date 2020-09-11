using System.Linq;
using AutoMapper;
using Core.Interfaces.Security;
using Core.ViewModels;
using Data;
using Microsoft.EntityFrameworkCore;
using Middleware.Contexts;

namespace Core.Utils
{
    public class SocialLinkResolver : IValueResolver<ActivityAttendee, ActivityAttendeeViewModel, bool>
    {
        private readonly DataContext _dataContext;
        private readonly IAppUserService _appUserService;

        public SocialLinkResolver(DataContext dataContext, IAppUserService appUserService)
        {
            _dataContext = dataContext;
            _appUserService = appUserService;
        }

        public bool Resolve(
            ActivityAttendee source,
            ActivityAttendeeViewModel destination,
            bool destMember,
            ResolutionContext context)
        {
            var currentUser = _dataContext.Users
                .SingleOrDefaultAsync(cu => cu.UserName == _appUserService.GetCurrentAppUserUsername()).Result;

            if (currentUser.Favourites.Any(link => link.TargetUserId == source.AppUserId))
                return true;

            return false;
        }
    }
}