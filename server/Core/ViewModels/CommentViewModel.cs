using System;

namespace Core.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Username { get; set; }
        public string AppUserDisplayName { get; set; }
        public string ImageURL { get; set; }
    }
}