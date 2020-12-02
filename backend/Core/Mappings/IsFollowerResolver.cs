using System.Linq;
using AutoMapper;
using Core.Interfaces;
using Data.Contexts;
using Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Mappings
{
    public class IsFollowerResolver : IValueResolver<ActivityAttendee, ActivityAttendeeViewModel, bool>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IUserAccessor _userAccessor;

        public IsFollowerResolver(DatabaseContext databaseContext, IUserAccessor userAccessor)
        {
            _databaseContext = databaseContext;
            _userAccessor = userAccessor;
        }

        public bool Resolve(ActivityAttendee source, ActivityAttendeeViewModel destination, bool destMember, ResolutionContext context)
        {
            var appUser = _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername()).Result;

            if (appUser.Followers.Any(x => x.FollowerId == source.AppUserId)) return true;

            return false;
        }
    }
}