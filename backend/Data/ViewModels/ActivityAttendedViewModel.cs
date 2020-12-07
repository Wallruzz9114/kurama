using System;

namespace Data.ViewModels
{
    public class ActivityAttendedViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime? Date { get; set; }
    }
}