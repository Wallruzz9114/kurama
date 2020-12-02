namespace Data.ViewModels
{
    public class ActivityAttendeeViewModel
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string PictureURL { get; set; }
        public bool IsHost { get; set; }
        public bool Following { get; set; }
        public bool IsFollower { get; set; }
    }
}