using Microsoft.AspNetCore.Identity;

namespace Data
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}