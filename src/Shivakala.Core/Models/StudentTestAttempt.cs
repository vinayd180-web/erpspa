using System;
using System.Collections.Generic;

namespace Shivakala.Core.Models
{
    public class StudentTestAttempt : BaseEntity
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int ExamPaperId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalMarks { get; set; }
        public int ObtainedMarks { get; set; }
        public string Status { get; set; } // InProgress, Completed, Submitted
        public List<StudentAnswer> Answers { get; set; }
    }
}
