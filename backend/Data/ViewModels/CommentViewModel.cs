using System;

namespace Data.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string PictureURL { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}