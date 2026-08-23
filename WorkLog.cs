using System;

namespace Shivakala.Core.Models
{
    public class WorkLog : BaseEntity
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public DateTime Date { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public string TopicCovered { get; set; }
        public string HomeworkGiven { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
    }
}
