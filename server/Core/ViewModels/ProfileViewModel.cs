using System.Collections.Generic;

namespace Data
{
    public class ProfileViewModel
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string ProfileImageURL { get; set; }
        public string Bio { get; set; }
        public bool FollowedByAppUser { get; set; }
        public int Followers { get; set; }
        public int Favourites { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}