namespace Core.ViewModels
{
    public class ActivityAttendeeViewModel
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureURL { get; set; }
        public bool IsHosting { get; set; }
        public bool IsFollowingAnyAttendees { get; set; }
    }
}