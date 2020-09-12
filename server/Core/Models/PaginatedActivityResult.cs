using System.Collections.Generic;
using Core.ViewModels;

namespace Core.Models
{
    public class PaginatedActivityResult
    {
        public int Count { get; set; }
        public IReadOnlyList<ActivityViewModel> Activities { get; set; }
    }
}