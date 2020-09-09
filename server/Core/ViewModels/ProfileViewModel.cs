using System.Collections.Generic;

namespace Data
{
    public class ProfileViewModel
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string ProfileImageURL { get; set; }
        public string Bio { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}