using System.Linq;
using AutoMapper;
using Core.Interfaces;
using Data.Contexts;
using Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Mappings
{
    public class FollowingResolver : IValueResolver<ActivityAttendee, ActivityAttendeeViewModel, bool>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IUserAccessor _userAccessor;

        public FollowingResolver(DatabaseContext databaseContext, IUserAccessor userAccessor)
        {
            _databaseContext = databaseContext;
            _userAccessor = userAccessor;
        }

        public bool Resolve(ActivityAttendee source, ActivityAttendeeViewModel destination, bool destMember, ResolutionContext context)
        {
            var appUser = _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername()).Result;

            if (appUser.UsersFollowed.Any(x => x.UserFollowedId == source.AppUserId)) return true;

            return false;
        }
    }
}