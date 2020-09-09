using System;
using System.Collections.Generic;

namespace Core.ViewModels
{
    public class ActivityViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Catagory { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public IReadOnlyList<ActivityAttendeeViewModel> ActivityAttendees { get; set; }
        public IReadOnlyList<CommentViewModel> Comments { get; set; }
    }
}