using System.Collections.Generic;
using Models;

namespace Data.ViewModels
{
    public class ProfileViewModel
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string PictureURL { get; set; }
        public string Bio { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}