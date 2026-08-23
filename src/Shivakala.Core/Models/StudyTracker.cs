using System;

namespace Shivakala.Core.Models
{
    public class StudyTracker : BaseEntity
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string TopicCovered { get; set; }
        public int HoursStudied { get; set; }
        public string DifficultyLevel { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
