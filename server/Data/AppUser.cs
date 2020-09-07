using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Data
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public virtual IReadOnlyList<ActivityAttendee> ActivityAttendees { get; set; }
    }
}