using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public virtual ICollection<ActivityAttendee> ActivityAttendees { get; set; }
    }
}