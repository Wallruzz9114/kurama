using System;

namespace Data
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public virtual AppUser Author { get; set; }
        public virtual Activity Activity { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}