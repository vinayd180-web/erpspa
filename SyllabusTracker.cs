using System;

namespace Shivakala.Core.Models
{
    public class SyllabusTracker : BaseEntity
    {
        public string Class { get; set; }
        public string Subject { get; set; }
        public string Chapter { get; set; }
        public string Topic { get; set; }
        public DateTime PlannedDate { get; set; }
        public DateTime ActualDate { get; set; }
        public bool IsCovered { get; set; }
        public string CoveredBy { get; set; }
        public string Remarks { get; set; }
        public int StudentId { get; set; } // For individual student tracking
    }
}
